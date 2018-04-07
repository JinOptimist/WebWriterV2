var bookMap = (function () {
    console.log('bookMap is load');
    var BlockSize = { Width: Const.ChapterSize.Width + Const.ChapterSize.Padding, Height: Const.ChapterSize.Height + Const.ChapterSize.Padding };
    var CanvasSize = {};
    var FontSize = Const.FontSize;
    var stage = {};
    var layer = {};
    var actions = {};

    var selectedChapter;

    var levels = [];
    var frontChapters = [];
    var fakeChapterId = -1;

    /* ******************************* events ******************************* */
    function onAddChapterClick(obj) {
        selectedChapter = undefined;
        var chapterId = obj.currentTarget.parent.chapter.LinksToThisEvent[0].FromId;
        console.log('bookMap onAddChapterClick. chapter.Id - ' + chapterId);
        actions.addChapter(chapterId);
    }

    function onEditChapterClick(obj) {
        var chapter = obj.currentTarget.chapter;
        console.log('bookMap onEditChapterClick. chapter.Id - ' + chapter.Id);
        actions.moveToEditChapter(chapter.Id);
    }

    function onRemoveChapterClick(obj) {
        var chapter = obj.currentTarget.parent.chapter;
        console.log('bookMap onRemoveChapterClick. chapter.Id - ' + chapter.Id);
        if (actions.remove(chapter)) {
            selectedChapter = undefined;
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
        var selectedChapterId = selectedChapter.Id;
        var clickedChapterId = obj.currentTarget.chapter.Id;
        var arrow = isSelectedChapterParent
            ? getArrowBetweenTwoChater(selectedChapterId, clickedChapterId)
            : getArrowBetweenTwoChater(clickedChapterId, selectedChapterId);
        actions.removeLink(arrow.linkId);
        onRightClick();
    }

    function onRightClick() {
        if (selectedChapter) {
            var index = frontChapters.findIndex(x => x.Id === fakeChapterId);
            var oldOwnerIdFakeChapter = frontChapters[index].LinksToThisEvent[0].FromId;
            frontChapters.splice(index, 1);
            var gr = getGroupByChapterId(fakeChapterId);
            gr.destroy();

            index = frontChapters.findIndex(x => x.Id === oldOwnerIdFakeChapter);
            var oldOwnerFakeChapter = frontChapters[index];
            index = oldOwnerFakeChapter.LinksFromThisEvent.findIndex(x => x.ToId === fakeChapterId);
            oldOwnerFakeChapter.LinksFromThisEvent.splice(index, 1);
        }
        selectedChapter = undefined;
        redraw();
    }

    function onDragChapterGroup(pos) {
        var parentsGroup = getParentsCanvasObj(this.chapter);

        // Remove arrow from parents to curent
        for (var i = 0; i < parentsGroup.length; i++) {
            var parent = parentsGroup[i];
            var arrow = getArrowBetweenTwoChater(parent.chapter.Id, this.chapter.Id);
            if (arrow) {
                arrow.remove();
                removeArrowDrawnRecord(parent, this);
            }
        }

        // Redraw arrow from current to children
        for (var i = 0; i < this.chapter.LinksFromThisEvent.length; i++) {
            var link = this.chapter.LinksFromThisEvent[i];
            var arrow = getArrowBetweenTwoChater(this.chapter.Id, link.ToId);
            if (!arrow) {
                continue;
            }
            
            arrow.remove();
            var child = getGroupByChapterId(link.ToId);
            removeArrowDrawnRecord(this, child);

            var arrow = drawShapes.drawArrow(this, child);
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

                this.chapter.LinksFromThisEvent.forEach(l => redrawArrow(this.chapter.Id, l.ToId, highlightNewElement));
                this.chapter.LinksToThisEvent.forEach(l => redrawArrow(l.FromId, this.chapter.Id, highlightNewElement));
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
        current.LinksFromThisEvent.push(link);

        var fakeChapter = {
            Id: fakeChapterId,
            Name: '',
            Desc: '',
            LinksFromThisEvent: [],
            LinksToThisEvent: [link],
            Level: current.Level + 1,
            Weight: 1
        };
        frontChapters.push(fakeChapter);
        redraw();
    }

    function onCreatingNewLink(obj) {
        actions.createLink(selectedChapter.Id, obj.currentTarget.chapter.Id);
        layer.draw();
    }
    /* ******************************* END ******************************* */

    function removeArrowDrawnRecord(parent, child) {
        var i = parent.drawnChildren.indexOf(child.chapter.Id);
        parent.drawnChildren[i] = undefined;

        i = child.drawnParents.indexOf(parent.chapter.Id);
        child.drawnParents[i] = undefined;
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
        group.chapterIndexX =  x;
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

        var textShape = drawShapes.drawText(chapter);
        group.add(textShape);

        layer.add(group);

        var arrows = drawShapes.drawArrows(group, parents);
        arrows.forEach(arrow => {
            cursorPointerHelper(arrow);
            layer.add(arrow);
        });
    }

    function draw(chapters) {
        levels = chapterProcessing.splitByLevels(chapters);
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
                }
            }
        }
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
        
        arrow.destroy();
        removeArrowDrawnRecord(parent, child);

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

    function reloadLayer(){
        layer.draw();
    }

    function redraw(chapters) {
        //stage.clear();
        //clearCache
        layer.destroyChildren();
        //layer.clearCache();
        reloadLayer();
        draw(frontChapters);
        reloadLayer();
    }

    /* public functions */
    function start(chapters, _actions, _canvasSize) {
        frontChapters = chapters;
        actions = _actions;
        CanvasSize = _canvasSize;
        stage = new Konva.Stage({
            container: 'nicePic',
            width: CanvasSize.width,
            height: CanvasSize.height,
            draggable: true
        });
        layer = new Konva.Layer();
        draw(chapters);
        stage.add(layer);
    }

    function resize(scale) {
        stage.scale({ x: scale, y: scale });
        stage.draw();
    }

    return {
        start: start,
        rightClick: onRightClick,
        resize: resize,
        frontChapters: () => frontChapters
    };
})();