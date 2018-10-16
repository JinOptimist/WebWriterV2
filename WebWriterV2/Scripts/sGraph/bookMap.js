var bookMap = (function () {
    console.log('bookMap is load');
    var BlockSize = { Width: Const.ChapterSize.Width + Const.ChapterSize.Padding, Height: Const.ChapterSize.Height + Const.ChapterSize.Padding };
    var CanvasSize = {};
    var FontSize = Const.FontSize;
    var stage = {};
    var layer = {};
    var globalVariableLayer = {};
    var actions = {};

    var selectedChapter;

    var levels = [];
    var frontChapters = [];
    var fakeChapterId = -1;
    var fronBook = {};
    var user = {};

    var debug = false;

    /* ******************************* events ******************************* */
    function onAddChapterClick(obj) {
        selectedChapter = undefined;
        var chapterId = obj.currentTarget.parent.chapter.LinksToThisChapter[0].FromId;
        console.log('bookMap onAddChapterClick. chapter.Id - ' + chapterId);
        actions.addChapter(chapterId, afterUpdateChapter);
    }

    function onEditChapterClick(obj) {
        var chapter = obj.currentTarget.chapter;
        console.log('bookMap onEditChapterClick. chapter.Id - ' + chapter.Id);
        actions.moveToEditChapter(chapter.Id, afterUpdateChapter);
    }

    function onRemoveChapterClick(obj) {
        var chapter = obj.currentTarget.parent.chapter;
        console.log('bookMap onRemoveChapterClick. chapter.Id - ' + chapter.Id);
        if (actions.remove(chapter)) {
            selectedChapter = undefined;
            removeDrawnChapter(fakeChapterId);
            removeDrawnChapter(chapter.Id);
            redraw();
        }
    }

    function onMainButtonClick(obj) {
        switch (obj.currentTarget.state) {
            case drawShapes.chapterStateType.Initial:
                onInitButtonClick(obj);
                break;
            case drawShapes.chapterStateType.FakeNew:
                onAddChapterClick(obj);
                break;
            case drawShapes.chapterStateType.Selected:
                onRemoveChapterClick(obj);
                break;
            case drawShapes.chapterStateType.AvailableToLink:
                onCreatingNewLink(obj);
                break;
            case drawShapes.chapterStateType.Parent:
                onRemoveLink(obj, false);
                break;
            case drawShapes.chapterStateType.Child:
                onRemoveLink(obj, true);
                break;
            default:
                break;
        }
    }

    function onRemoveLink(obj, isSelectedChapterParent) {
        var parentGroup, childGroup;
        var selectedChapterId = selectedChapter.Id;
        var clickedChapterId = obj.currentTarget.chapter.Id;
        if (isSelectedChapterParent) {
            parentGroup = getGroupByChapterId(selectedChapterId);
            childGroup = getGroupByChapterId(clickedChapterId);
        } else {
            parentGroup = getGroupByChapterId(clickedChapterId);
            childGroup = getGroupByChapterId(selectedChapterId);
        }

        var linkId = removeArrow(parentGroup, childGroup);
        actions.removeLink(linkId);

        removeFromAndToRecord(parentGroup.chapter.Id, childGroup.chapter.Id);

        onRightClick();
    }

    function onCreatingNewLink(obj) {
        var from = selectedChapter;
        var to = obj.currentTarget.chapter;
        actions.createLink(from.Id, to.Id, onSuccessedLinkCreate);
        //if (from.Level >= to.Level) {
        //    layer.destroyChildren();
        //    reloadLayer();
        //    redraw();
        //}
    }

    function onSuccessedLinkCreate(link) {
        var fromId = link.FromId;
        var toId = link.ToId;
        var fromChapter = frontChapters.find(x => x.Id === fromId);
        fromChapter.LinksFromThisChapter.push(link);

        var toChapter = frontChapters.find(x => x.Id === toId);
        toChapter.LinksToThisChapter.push(link);

        onRightClick();
    }

    function onRightClick() {
        if (selectedChapter) {
            removeDrawnChapter(fakeChapterId);
        }
        selectedChapter = undefined;
        redraw();
    }

    function onDragChapterGroup(pos) {
        var arrow;
        var parentsGroup = getParentsCanvasObj(this.chapter);

        // Remove arrow from parents to curent
        for (var i = 0; i < parentsGroup.length; i++) {
            var parent = parentsGroup[i];
            arrow = getArrowBetweenTwoChater(parent.chapter.Id, this.chapter.Id);
            if (arrow) {
                removeArrow(parent, this);
            }
        }

        // Redraw arrow from current to children
        for (i = 0; i < this.chapter.LinksFromThisChapter.length; i++) {
            var link = this.chapter.LinksFromThisChapter[i];
            arrow = getArrowBetweenTwoChater(this.chapter.Id, link.ToId);
            if (!arrow) {
                continue;
            }

            var child = getGroupByChapterId(link.ToId);
            removeArrow(this, child)

            arrow = drawShapes.drawArrow(this, child);
            cursorPointerHelper(arrow);
            layer.add(arrow);
        }

        // Draw arrow from parents to curent
        var arrows = drawShapes.drawArrows(this, parentsGroup);
        arrows.forEach(arrow => {
            cursorPointerHelper(arrow);
            layer.add(arrow);
        });

        return {
            x: pos.x,
            y: this.getAbsolutePosition().y
        };
    }

    function cursorPointerHelper(obj) {
        obj.on('mouseenter', function () {
            stage.container().style.cursor = 'pointer';
        });

        obj.on('mouseleave', function () {
            stage.container().style.cursor = 'default';
        });
    }

    function onGroupClick(obj) {
        console.log('bookMap onChapterClick. chapter.Id - ' + this.chapter.Id);

        var newArrow = stage.find('#newArrow')[0];
        if (newArrow) {
            finishCreatingNewLink(newArrow, this.chapter.Id)
        } else {
            var oldHighlightGroups = getHighlightGroup();
            oldHighlightGroups.forEach(x => redrawGroup(x, false));

            var oldHighlightArrow = getHighlightArrow();
            oldHighlightArrow.forEach(ar => redrawArrow(ar.twoSides.parentId, ar.twoSides.childId, false));

            var highlightNewElement = !obj.currentTarget.isHighlight;
            if (highlightNewElement) {
                redrawGroup(obj.currentTarget, highlightNewElement);

                this.chapter.LinksFromThisChapter.forEach(l => redrawArrow(this.chapter.Id, l.ToId, highlightNewElement));
                this.chapter.LinksToThisChapter.forEach(l => redrawArrow(l.FromId, this.chapter.Id, highlightNewElement));
            }

            layer.draw();
        }
    }

    function onInitButtonClick(obj) {
        var current = obj.currentTarget.parent.chapter;
        selectedChapter = current;
        var link = {
            FromId: current.Id,
            ToId: fakeChapterId,
        };
        current.LinksFromThisChapter.push(link);

        var fakeChapter = {
            Id: fakeChapterId,
            Name: '',
            Desc: '',
            LinksFromThisChapter: [],
            LinksToThisChapter: [link],
            Level: current.Level + 1,
            Weight: 1
        };
        frontChapters.push(fakeChapter);
        redraw();
    }
    /* ******************************* END ******************************* */

    function removeDrawnChapter(chapterId) {
        var indexOfChapter = frontChapters.findIndex(x => x.Id === chapterId);
        var ownerChapterId = frontChapters[indexOfChapter].LinksToThisChapter[0].FromId;
        frontChapters.splice(indexOfChapter, 1);
        var gr = getGroupByChapterId(chapterId);
        gr.destroy();

        removeFromAndToRecord(ownerChapterId, chapterId);
    }

    function removeFromAndToRecord(fromId, toId) {
        var fromChapter = frontChapters.find(x => x.Id === fromId);
        var index = fromChapter.LinksFromThisChapter.findIndex(x => x.ToId === toId);
        fromChapter.LinksFromThisChapter.splice(index, 1);

        var toChapter = frontChapters.find(x => x.Id === toId);
        if (!toChapter) {
            return;
        }

        index = toChapter.LinksToThisChapter.findIndex(x => x.FromId === fromId);
        toChapter.LinksToThisChapter.splice(index, 1);
    }

    function afterUpdateChapter(newChapter) {
        var chapter = frontChapters.find(x => x.Id == newChapter.Id);
        if (chapter) {
            chapter.Name = newChapter.Name;
            updateTextForLinks(newChapter.LinksFromThisChapter);
            updateTextForLinks(newChapter.LinksToThisChapter);
        } else {
            // if we update not exist chapter, we add new one
            var parentOfNewChapterId = newChapter.LinksToThisChapter[0].FromId;
            var parentOfNewChapter = frontChapters.find(x => x.Id == parentOfNewChapterId);
            newChapter.Level = parentOfNewChapter.Level + 1;
            frontChapters.push(newChapter);
            var link = newChapter.LinksToThisChapter[0];
            parentOfNewChapter.LinksFromThisChapter.push(link);

            removeDrawnChapter(fakeChapterId);
        }

        redraw();
    }

    function updateTextForLinks(links) {
        if (!links) {
            return;
        }

        for (var i = 0; i < links.length; i++) {
            var link = links[i];
            var from = frontChapters.find(x => x.Id == link.FromId);
            var to = frontChapters.find(x => x.Id == link.ToId);

            from.LinksFromThisChapter.find(x => x.Id == link.Id).Text = link.Text;
            to.LinksToThisChapter.find(x => x.Id == link.Id).Text = link.Text;
        }
    }

    function removeArrow(parentGroup, childGroup) {
        var arrow = getArrowBetweenTwoChater(parentGroup.chapter.Id, childGroup.chapter.Id);
        var linkId = arrow.linkId;
        arrow.destroy();

        var i = parentGroup.drawnChildren.indexOf(childGroup.chapter.Id);
        parentGroup.drawnChildren[i] = undefined;

        i = childGroup.drawnParents.indexOf(parentGroup.chapter.Id);
        childGroup.drawnParents[i] = undefined;

        return linkId;
    }

    function drawChapterGroup(x, y, chapter, isHighlight) {
        //var centerX = canvas.width / 2;
        var parents = getParentsCanvasObj(chapter);
        var centerX = getCenterByParents(parents);

        var chapterX = (x / 2) * BlockSize.Width + Const.ChapterSize.Padding + centerX;
        var chapterY = y * BlockSize.Height + Const.ChapterSize.Padding;
        var isCrossed = false;
        while (isCrossOtherChapter(chapterX, chapterY)) {
            chapterX += 10;
            isCrossed = true;
        }
        if (isCrossed) {
            console.warn("Something go wrong. Two chapters are crossed. x - %d y - %d, chapter.Id - %d. ", x, y, chapter.Id, chapter);
        }

        var group = new Konva.Group({
            //draggable: true,
            x: chapterX,
            y: chapterY,
            dragBoundFunc: onDragChapterGroup,
            id: 'chGr' + chapter.Id,
        });
        group.chapter = chapter;
        group.drawnChildren = [];
        group.drawnParents = [];
        group.chapterIndexX = x;
        group.chapterIndexY = y;
        group.logicType = drawShapes.shapeLogicType.ChapterGroup;
        group.isHighlight = isHighlight;
        //group.on('click', onGroupClick);
        group.on('dblclick', onEditChapterClick)

        var chapterBox = drawShapes.drawChapter(chapter, isHighlight);
        group.add(chapterBox);

        var state = chapterProcessing.calcState(chapter, selectedChapter);
        var mainButton = drawShapes.drawMainButton(chapter, onMainButtonClick, state, reloadLayer);
        cursorPointerHelper(mainButton);
        group.add(mainButton);

        var textShape = drawShapes.drawText(chapter, user);
        group.add(textShape);

        layer.add(group);
    }

    function drawArrows(arrows) {
        for (var i = 0 ; i < arrows.length; i++) {
            var arrow = arrows[i];
            var child = getGroupByChapterId(arrow.FromId);
            var parent = getGroupByChapterId(arrow.ToId);

            var arrowShape = drawShapes.drawArrow(child, parent);
            layer.add(arrowShape);
        }
    }

    function draw() {
        var links = [];
        levels = chapterProcessing.splitByLevels(frontChapters);
        //group chapters by parents.
        for (var y = 1; y < levels.length; y++) {
            var level = levels[y];
            chaptersGroupByParent = chapterProcessing.groupByParent(level);

            for (var gr = 0; gr < chaptersGroupByParent.length; gr++) {
                var chapterGroupByParent = chaptersGroupByParent[gr];

                var fullLevelSize = 0;
                chapterGroupByParent.forEach(function (ch) {
                    fullLevelSize += ch.Weight;
                });
                var currentX = 1 - fullLevelSize;

                for (var i = 0; i < chapterGroupByParent.length; i++) {
                    var chapter = chapterGroupByParent[i];
                    var x = currentX + chapter.Weight - 1;
                    drawChapterGroup(x, y - 1, chapter);
                    currentX += chapter.Weight * 2;

                    chapter.LinksFromThisChapter.forEach(x => links.push(x));
                }
            }
        }

        drawGlobalVariableGroup();
        reloadLayer();
        drawArrows(links);
        actions.validate(frontChapters);

        if (debug) {
            drawScaleDot();
        }
    }

    function drawGlobalVariableGroup() {
        var y = 10;
        fronBook.States.forEach(function (state) {
            var textShape = new Konva.Text({
                x: 10,
                y: y,
                width: 200,
                FontSize: Const.FontSize,
                fontFamily: "Lato",
                text: state.Name,
                fill: Const.FontColor,
                align: 'left'
            });
            globalVariableLayer.add(textShape);

            y += 20;
        });
        
    }

    /* ******************************* stage helper ******************************* */
    function removeButtonForGroup(group) {
        var itemToDestroy = [];
        group.children.forEach(function (item) {
            if (item.logicType == drawShapes.shapeLogicType.ChapterButton) {
                itemToDestroy.push(item);
            }
        });

        itemToDestroy.forEach(x => x.remove());
    }

    function getCenterByParents(parents) {
        if (!parents || parents.length < 1)
            return CanvasSize.width / 2 - BlockSize.Width / 2;

        var xCoordinates = parents.map(chapterBlock => chapterBlock.attrs.x);

        var min = Math.min.apply(Math, xCoordinates);
        var max = Math.max.apply(Math, xCoordinates);
        var result = (min + max) / 2;
        return result - Const.ChapterSize.Padding;
    }

    function isCrossOtherChapter(x, y) {
        var chapterWichAreCrossed = layer.children.filter(function (canvasItem) {
            if (!canvasItem.chapter)
                return false;
            var existItemX = Math.round(canvasItem.attrs.x);

            return canvasItem.attrs.y === y
                && existItemX > Math.round(x - BlockSize.Width)
                && existItemX < Math.round(x + BlockSize.Width);
        });

        return chapterWichAreCrossed.length > 0;
    }

    function getArrowBetweenTwoChater(parentId, childId) {
        return layer.children.filter(function (canvasItem) {
            if (!canvasItem.twoSides)
                return false;

            return canvasItem.twoSides.parentId === parentId && canvasItem.twoSides.childId === childId;
        })[0];
    }

    function getParentsCanvasObj(selectedChapter) {
        return layer.children.filter(function (canvasItem) {
            if (!canvasItem.chapter)
                return false;
            var chapter = canvasItem.chapter;
            return chapterProcessing.chaptersAreLinked(chapter, selectedChapter);
        });
    }

    function getHighlightArrow() {
        return layer.children.filter(function (canvasItem) {
            return canvasItem.logicType == drawShapes.shapeLogicType.Arrow
                && canvasItem.isHighlight;
        });
    }

    function getHighlightGroup() {
        return layer.children.filter(function (canvasItem) {
            return canvasItem.logicType == drawShapes.shapeLogicType.ChapterGroup
                && canvasItem.isHighlight;
        });
    }

    function getGroupByChapterId(chapterId) {
        return stage.find('#chGr' + chapterId)[0];
    }

    function redrawArrow(fromId, toId, highlight) {
        var arrow = getArrowBetweenTwoChater(fromId, toId);
        if (!arrow)
            return;

        var parent = getGroupByChapterId(fromId);
        var child = getGroupByChapterId(toId);

        removeArrow(parent, child)

        arrow = drawShapes.drawArrow(parent, child, highlight);
        cursorPointerHelper(arrow);
        layer.add(arrow);
    }

    function redrawActiveNewArrow() {
        var newArrow = stage.find('#newArrow')[0];
        var points = newArrow.points();
        var x1 = points[0];
        var y1 = points[1];
        var fromChapterId = newArrow.fromChapterId;
        newArrow.destroy();

        var mousePosition = stage.getPointerPosition();
        var x2 = mousePosition.x;
        var y2 = mousePosition.y - 5;

        var arrow = new Konva.Arrow({
            points: [x1, y1, x2, y2],
            stroke: "#0aa",
            strokeWidth: 2,
            lineCap: "round",
            fill: 'black',
            pointerLength: 8,
            pointerWidth: 12,
            id: 'newArrow'
        });
        arrow.fromChapterId = fromChapterId;

        layer.add(arrow);
        layer.draw();
    }

    function redrawGroup(group, isHighlight) {
        var x = group.chapterIndexX;
        var y = group.chapterIndexY;
        var chapter = group.chapter;

        group.drawnParents.forEach(function (parentId) {
            var parent = getGroupByChapterId(parentId);
            var indexOfCurrentChapter = parent.drawnChildren.indexOf(chapter.Id);
            parent.drawnChildren[indexOfCurrentChapter] = undefined;
        });

        group.destroy();

        drawChapterGroup(x, y, chapter, isHighlight);
    }
    /* ******************************* END ******************************* */

    function reloadLayer() {
        layer.draw();
        globalVariableLayer.draw();
    }

    function redraw() {
        layer.destroyChildren();
        globalVariableLayer.destroyChildren();
        reloadLayer();
        draw();
        reloadLayer();
    }

    /* public functions */
    function start(book, _actions, _canvasSize, _user) {
        user = _user;
        fronBook = book;
        frontChapters = book.Chapters;
        actions = _actions;
        CanvasSize = _canvasSize;
        stage = new Konva.Stage({
            container: 'nicePic',
            width: CanvasSize.width,
            height: CanvasSize.height,
            draggable: true
        });
        layer = new Konva.Layer({
            draggable: true
        });
        globalVariableLayer = new Konva.Layer();
        //stage.add(layer, globalVariableLayer);
        stage.add(layer);
        draw();
        reloadLayer();
    }

    function resize(newScale) {
        var oldScale = layer.scaleX();

        var pointer = stage.getPointerPosition();

        var mousePointTo = {
            x: pointer.x / oldScale - layer.getAbsolutePosition().x / oldScale,
            y: pointer.y / oldScale - layer.getAbsolutePosition().y / oldScale,
        };

        layer.scale({ x: newScale, y: newScale });
        pointer = stage.getPointerPosition();
        var newPos = {
            x: -(mousePointTo.x - pointer.x / newScale) * newScale - stage.position().x,
            y: -(mousePointTo.y - pointer.y / newScale) * newScale - stage.position().y
        };

        layer.position(newPos);
        layer.draw();
    }

    function drawScaleDot() {
        var mult = 100;
        for (var x = 0; x < 30; x++) {
            for (var y = 0; y < 30; y++) {
                var circle = new Konva.Circle({
                    x: x * mult,
                    y: y * mult,
                    radius: 2,
                    fill: '',
                    stroke: (x+y)%2 == 1 ? 'black' : 'red',
                    strokeWidth: 1
                });
                var text = new Konva.Text({
                    x: x * mult + 5,
                    y: y * mult - 5,
                    text: 'x:' + (x * mult) + ' y:' + (y * mult),
                    fontSize: 10,
                });

                layer.add(text);
                layer.add(circle);
            }
        }
    }

    return {
        start: start,
        rightClick: onRightClick,
        resize: resize,
        // for debug
        frontChapters: () => frontChapters
    };
})();