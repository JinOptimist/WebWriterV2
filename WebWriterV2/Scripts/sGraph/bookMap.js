var bookMap = (function () {
    console.log('bookMap is load');
    var ChapterSize = Const.ChapterSize;
    var BlockSize = { Width: ChapterSize.Width + ChapterSize.Padding, Height: ChapterSize.Height + ChapterSize.Padding };
    var AddButtonSize = Const.AddButtonSize;
    var canvasSize = {};
    var FontSize = Const.FontSize;

    var stage = {};
    var layer = {};
    var actions = {};

    var levels = [];

    var useDirectArrow = false;

    /* events */
    function onChapterClick(obj) {
        console.log('bookMap onChapterClick. chapter.Id - ' + this.chapter.Id);
        actions.moveToEditChapter(this.chapter.Id);
    }

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
        actions.remove(this.relatedRemove.chapter.Id);
    }

    function onDragChpaterGroup(pos) {

        var drableGroupParents = getParrentsCanvasObj(this.chapter.Id);

        // Remove arrow from parents to curent
        for (var i = 0; i < drableGroupParents.length; i++) {
            var parent = drableGroupParents[i];
            var arrow = getArrowBetweenTwoChater(parent.chapter.Id, this.chapter.Id);
            if (arrow) {
                arrow.remove();

                this.drawParentCount--;
                parent.drawChildCount--;
            }
        }

        // Redraw arrow from current to children
        for (var i = 0; i < this.chapter.LinksFromThisEvent.length; i++) {
            var link = this.chapter.LinksFromThisEvent[i];
            var arrow = getArrowBetweenTwoChater(this.chapter.Id, link.ToId);
            if (!arrow) {
                continue;
            }
            var childPoints = arrow.points();
            var childX = childPoints[childPoints.length - 2];
            var childY = childPoints[childPoints.length - 1];

            arrow.remove();

            var child = getChapterCanvasObjById(link.ToId);
            this.drawChildCount--;
            child.drawParentCount--;

            var arrow = useDirectArrow
                ? drawShapes.drawDirectArrow(this, child)
                : drawShapes.drawArrow(this, child);
            layer.add(arrow);
        }

        // Draw arrow from parents to curent
        var arrows = useDirectArrow
            ? drawShapes.drawDirectArrows(this, drableGroupParents)
            : drawShapes.drawArrows(this, drableGroupParents);
        arrows.forEach(arrow => layer.add(arrow));

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
    /* events END */

    function drawChapterBlock(x, y, chapter) {
        //var centerX = canvas.width / 2;
        var parents = getParrentsCanvasObj(chapter.Id);
        var centerX = getCenterByParents(parents);

        var chapterX = (x / 2) * BlockSize.Width + ChapterSize.Padding + centerX;
        var chapterY = y * BlockSize.Height + ChapterSize.Padding;
        var isCrossed = false;
        while (isCrossOtherChapter(chapterX, chapterY)) {
            chapterX += 10;
            isCrossed = true;
        }
        if (isCrossed) {
            console.warn("Something go wrong. Two chapters are crossed. x - %d y - %d, chapter.Id - %d. ", x, y, chapter.Id, chapter);
        }

        var group = new Konva.Group({
            draggable: true,
            x: chapterX,
            y: chapterY,
            dragBoundFunc: onDragChpaterGroup
        });
        group.chapter = chapter;
        group.drawChildCount = -1;
        group.drawParentCount = -1;

        var chapterBox = drawShapes.drawChapter(chapter);
        group.add(chapterBox);

        var addButton = drawShapes.drawAddChapterButoon(chapter, onAddChapterClick, cursorPointerHelper);
        group.add(addButton);
        var editButton = drawShapes.drawEditChapterButoon(chapter, onEditChapterClick, cursorPointerHelper);
        group.add(editButton);
        var removeButton = drawShapes.drawRemoveChapterButoon(chapter, onRemoveChapterClick, cursorPointerHelper);
        group.add(removeButton);

        var textShape = drawShapes.drawText(chapter);
        group.add(textShape);

        layer.add(group);

        var arrows = useDirectArrow
            ? drawShapes.drawDirectArrows(group, parents)
            : drawShapes.drawArrows(group, parents);
        arrows.forEach(arrow => layer.add(arrow));
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
                    drawChapterBlock(x, y - 1, chapter);
                    currentX += chapter.Weight * 2;
                }
            }
        }
    }

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

    function updateChapterSize(maxWeight) {
        ChapterSize.Width = canvasSize.width / (maxWeight + 2);
        BlockSize.Width = ChapterSize.Width + ChapterSize.Padding;
        Const.ChapterSize = ChapterSize;
    }

    function findParents(chapters, chapterId) {
        return chapters.filter(function (chapter) {
            return chapterContainsLinkToCurrentChapter(chapter, chapterId);
        });
    }

    function getParrentsCanvasObj(chapterId) {
        return layer.children.filter(function (canvasItem) {
            if (!canvasItem.chapter)
                return false;
            var chapter = canvasItem.chapter;
            return chapterContainsLinkToCurrentChapter(chapter, chapterId);
        });
    }

    function getChapterCanvasObjById(chapterId) {
        return layer.children.filter(function (canvasItem) {
            if (!canvasItem.chapter)
                return false;
            return canvasItem.chapter.Id === chapterId;
        })[0];
    }

    function getArrowBetweenTwoChater(parentId, childId) {
        return layer.children.filter(function (canvasItem) {
            if (!canvasItem.twoSides)
                return false;

            return canvasItem.twoSides.parentId === parentId && canvasItem.twoSides.childId === childId;
        })[0];
    }

    function chapterContainsLinkToCurrentChapter(curentChapter, destinationChapterId) {
        if (!curentChapter.LinksFromThisEvent || curentChapter.LinksFromThisEvent.length === 0)
            return false;
        return curentChapter.LinksFromThisEvent.filter(link => link.ToId === destinationChapterId).length > 0;
    }

    function getCenterByParents(parents) {
        if (!parents || parents.length < 1)
            return canvasSize.width / 2 - BlockSize.Width / 2;

        var xCoordinates = parents.map(chapterBlock => chapterBlock.attrs.x);

        var min = Math.min.apply(Math, xCoordinates);
        var max = Math.max.apply(Math, xCoordinates);
        var result = (min + max) / 2;
        //if (min != max) {
        //    result -= BlockSize.Width / 2;
        //}
        return result - ChapterSize.Padding;
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

    function isCrossOtherChapter(x, y) {
        var chapterWichAreCrossed = layer.children.filter(function (canvasItem) {
            if (!canvasItem.chapter)
                return false;
            var existItemX = canvasItem.attrs.x;
            return canvasItem.attrs.y === y && existItemX > x - BlockSize.Width && existItemX < x + BlockSize.Width;
        });

        return chapterWichAreCrossed.length > 0;
    }

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
        redraw: redraw
    };
})();