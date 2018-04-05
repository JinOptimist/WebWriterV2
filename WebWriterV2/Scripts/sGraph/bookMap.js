var bookMap = (function () {
    console.log('bookMap is load');
    var BlockSize = { Width: Const.ChapterSize.Width + Const.ChapterSize.Padding, Height: Const.ChapterSize.Height + Const.ChapterSize.Padding };
    var canvasSize = {};
    var FontSize = Const.FontSize;

    var stage = {};
    var layer = {};
    var actions = {};

    var levels = [];

    var useDirectArrow = false;

    /* ******************************* events ******************************* */
    function onAddChapterClick(obj) {
        console.log('bookMap onAddChapterClick. chapter.Id - ' + this.relatedAdd.chapter.Id);
        actions.addChapter(this.relatedAdd.chapter.Id);
    }

    function onEditChapterClick(obj) {
        console.log('bookMap onEditChapterClick. chapter.Id - ' + this.relatedEdit.chapter.Id);
        actions.moveToEditChapter(this.relatedEdit.chapter.Id);
    }

    function onRemoveChapterClick(obj) {
        console.log('bookMap onRemoveChapterClick. chapter.Id - ' + this.relatedRemove.chapter.Id);
        actions.remove(this.relatedRemove.chapter);
    }

    function onAddLinkClick(obj) {
        var chapterId = this.chapter.Id;
        console.log('bookMap onAddLinkClick. chapter.Id - ' + chapterId);
        var chapterBlock = getGroupByChapterId(chapterId);

        var x1 = chapterBlock.attrs.x;
        var y1 = chapterBlock.attrs.y;
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
            id: 'newArrow',
        });
        arrow.fromChapterId = chapterId;

        layer.add(arrow);
        layer.draw();

        stage.on('mousemove', redrawActiveNewArrow);

        obj.cancelBubble = true;
    }

    function onArrowClick(obj) {
        var arrow = obj.target;
        actions.removeLink(arrow.linkId);
    }

    function onRightClick() {
        var newArrow = stage.find('#newArrow')[0];
        stage.off('mousemove');
        newArrow.destroy();
        layer.draw();
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

            var arrow = useDirectArrow
                ? drawShapes.drawDirectArrow(this, child)
                : drawShapes.drawArrow(this, child, onArrowClick);
            cursorPointerHelper(arrow);
            layer.add(arrow);
        }

        // Draw arrow from parents to curent
        var arrows = useDirectArrow
            ? drawShapes.drawDirectArrows(this, parentsGroup)
            : drawShapes.drawArrows(this, parentsGroup, onArrowClick);
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

    function onGroupMouseEnter(obj) {
        var group = obj.currentTarget;
        console.log('onGroupMouseEnter ' + group.chapter.id);
        drawButtonForGroup(group, group.chapter);
    }

    function onGroupMouseLeave(obj) {
        var group = obj.currentTarget;
        console.log('onGroupMouseLeave ' + group.chapter.id);
        removeButtonForGroup(group);
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
        group.on('click', onGroupClick);
        group.on('mouseenter', onGroupMouseEnter);
        group.on('mouseleave', onGroupMouseLeave);

        var chapterBox = drawShapes.drawChapter(chapter, isHighlight);
        group.add(chapterBox);

        if (isHighlight) {
            drawButtonForGroup(group, chapter);
        }

        var textShape = drawShapes.drawText(chapter);
        group.add(textShape);

        layer.add(group);

        var arrows = useDirectArrow
            ? drawShapes.drawDirectArrows(group, parents, onArrowClick)
            : drawShapes.drawArrows(group, parents, onArrowClick);
        arrows.forEach(arrow => {
            cursorPointerHelper(arrow);
            layer.add(arrow);
        });
    }

    function draw(chapters, newScale) {
        splitByLevels(chapters);

        levels = splitByLevels(chapters);
        //group chapters by parents.
        for (var y = 1; y < levels.length; y++) {
            var level = levels[y];
            chaptersGroupByParent = groupByParent(level);

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

    function updateChapterSize(maxWeight) {
        Const.ChapterSize.Width = canvasSize.width / (maxWeight + 2);
        BlockSize.Width = Const.ChapterSize.Width + Const.ChapterSize.Padding;
        Const.ChapterSize = Const.ChapterSize;
    }
    
    /* ******************************* tools to work with FronModels ******************************* */
    function splitByLevels(chapters) {
        var maxWeight = 1;
        var levels = [];
        for (var i = 0; i < chapters.length; i++) {
            var chapter = chapters[i];
            chapter.parents = findParents(chapters, chapter.Id);
            var depth = chapter.Level;
            if (depth === 0) {
                //chapters without parent are ignored
                continue;
            }

            if (chapter.Weight > maxWeight) {
                maxWeight = chapter.Weight;
            }

            var level = levels[depth];
            if (!level) {
                levels[depth] = [];
            }
            levels[depth].push(chapter);
        }

        updateChapterSize(maxWeight);

        return levels;
    }

    function chapterContainsLinkToCurrentChapter(curentChapter, destinationChapter) {
        if (!curentChapter.LinksFromThisEvent || curentChapter.LinksFromThisEvent.length === 0)
            return false;
        if (curentChapter.Level > destinationChapter.Level)
            return false;
        return curentChapter.LinksFromThisEvent.filter(link => link.ToId === destinationChapter.Id).length > 0;
    }
    
    function groupByParent(chapters) {
        if (chapters.length === 1)
            return [chapters];

        chapters = chapters.sort(function (a, b) {
            if (!a.parents)
                return -1;
            if (!b.parents)
                return 1;

            return a.parents[0].Id - b.parents[0].Id;
        });

        var result = [];
        var activeGroup = [];
        var groupByParentId = chapters[0].parents[0].Id;
        for (var i = 0; i < chapters.length; i++) {
            var ch = chapters[i];
            if (ch.parents[0].Id !== groupByParentId) {
                groupByParentId = ch.parents[0].Id;
                if (activeGroup.length > 0)
                    result.push(activeGroup);
                activeGroup = [];
            }
            activeGroup.push(ch);
        }
        result.push(activeGroup);

        return result;
    }

    function findParents(chapters, chapterId) {
        var currentChapter = chapters.find(x=>x.Id === chapterId);
        return chapters.filter(function (chapter) {
            return chapterContainsLinkToCurrentChapter(chapter, currentChapter);
        });
    }
    /* ******************************* END ******************************* */

    /* ******************************* stage helper ******************************* */
    function drawButtonForGroup(group, chapter) {
        var addButton = drawShapes.drawAddChapterButton(chapter, onAddChapterClick);
        cursorPointerHelper(addButton);
        group.add(addButton);
        var editButton = drawShapes.drawEditChapterButton(chapter, onEditChapterClick);
        cursorPointerHelper(editButton);
        group.add(editButton);
        var removeButton = drawShapes.drawRemoveChapterButton(chapter, onRemoveChapterClick);
        cursorPointerHelper(removeButton);
        group.add(removeButton);
        var addLinkButton = drawShapes.drawAddLinkButton(chapter, onAddLinkClick);
        cursorPointerHelper(addLinkButton);
        group.add(addLinkButton);

        layer.draw();
    }
    
    function removeButtonForGroup(group) {
        //for (var i = 0; i < group.children.length; i++) {
        //    group.children[i];
        //}
        var itemToDestroy = [];
        group.children.forEach(function (item) {
            if (item.logicType == drawShapes.shapeLogicType.ChapterButton) {
                itemToDestroy.push(item);
            }
        });

        itemToDestroy.forEach(x => x.destroy());

        layer.draw();
    }

    function getCenterByParents(parents) {
        if (!parents || parents.length < 1)
            return canvasSize.width / 2 - BlockSize.Width / 2;

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
            return chapterContainsLinkToCurrentChapter(chapter, selectedChapter);
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

    function finishCreatingNewLink(newArrow, chapterId) {
        if (newArrow.fromChapterId !== chapterId) {
            actions.createLink(newArrow.fromChapterId, chapterId);
        }

        stage.off('mousemove');
        newArrow.destroy();
        layer.draw();
    }

    function redrawArrow(fromId, toId, highlight) {
        var arrow = getArrowBetweenTwoChater(fromId, toId);
        if (!arrow)
            return;

        var parent = getGroupByChapterId(fromId);
        var child = getGroupByChapterId(toId);
        
        arrow.destroy();
        removeArrowDrawnRecord(parent, child);

        arrow = drawShapes.drawArrow(parent, child, onArrowClick, highlight);
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

    /* public functions */
    function start(chapters, newScale, _actions, _canvasSize) {
        actions = _actions;
        canvasSize = _canvasSize;
        stage = new Konva.Stage({
            container: 'nicePic',
            width: canvasSize.width,
            height: canvasSize.height
        });
        layer = new Konva.Layer();

        draw(chapters, newScale);

        stage.add(layer);
    }

    function redraw(chapters, newScale) {
        if (canvas)
            canvas.destroy();
        start(chapters, newScale);
    }

    return {
        start: start,
        redraw: redraw,
        rightClick: onRightClick
    };
})();