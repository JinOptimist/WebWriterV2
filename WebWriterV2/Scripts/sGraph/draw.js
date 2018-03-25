var bookMap = (function () {
    console.log('bookMap is load');
    var ChapterSize = { Width: 100, Height: 40, Padding: 30 };
    var BlockSize = { Width: ChapterSize.Width + ChapterSize.Padding, Height: ChapterSize.Height + ChapterSize.Padding };
    var AddButtonSize = { Radius: 5, Padding: 3 };
    var canvasSize = {};
    var ArrowSize = 5;
    var FontSize = 12;

    var stage = {};
    var layer = {};
    var actions = {};

    var levels = [];

    var useDirectArrow = true;

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

    function onDragChpaterHroup(pos) {

        var drableGroupParents = getParrentsCanvasObj(this.chapter.Id);

        //remove arrow from parents to curent
        for (var i = 0; i < drableGroupParents.length; i++) {
            var parent = drableGroupParents[i];
            var arrow = getArrowBetweenTwoChater(parent.chapter.Id, this.chapter.Id);
            arrow.remove();
        }

        // remove arrow from current to children
        for (var i = 0; i < this.chapter.LinksFromThisEvent.length; i++) {
            var link = this.chapter.LinksFromThisEvent[i];
            var arrow = getArrowBetweenTwoChater(this.chapter.Id, link.ToId);
            var childPoints = arrow.points();
            var childX = childPoints[childPoints.length - 2];
            var childY = childPoints[childPoints.length - 1];

            arrow.remove();

            drawDirectArrow(pos.x, pos.y, childX, childY, this.chapter.Id, link.ToId);
        }

        drawDirectArrows(this.chapter, drableGroupParents, pos.x, pos.y);

        //arrow.twoSides = {
        //    parentId: parent.Id,
        //    childId: chapter.Id
        //};

        return {
            x: pos.x,
            y: pos.y
        };
    }

    /* draw section */
    function drawChapterBlock(x, y, chapter) {
        //var centerX = canvas.width / 2;
        var parents = getParrentsCanvasObj(chapter.Id);
        var centerX = getCenterByParents(parents);

        var chapterX = (x / 2) * BlockSize.Width + ChapterSize.Padding + centerX;
        var chapterY = y * BlockSize.Height + ChapterSize.Padding;
        while (isCrossOtherChapter(chapterX, chapterY)) {
            chapterX += 10;
        }

        var group = new Konva.Group({
            draggable: true,
            x: chapterX,
            y: chapterY,
            dragBoundFunc: onDragChpaterHroup
        });
        group.chapter = chapter;

        var chapterBox = drawChapter(chapter);
        group.add(chapterBox);

        var addButton = drawAddChapterButoon(chapter);
        group.add(addButton);
        var editButton = drawEditChapterButoon(chapter);
        group.add(editButton);
        var removeButton = drawRemoveChapterButoon(chapter);
        group.add(removeButton);

        var textShape = drawText(chapter);
        group.add(textShape);

        useDirectArrow
            ? drawDirectArrows(chapter, parents, chapterX, chapterY)
            : drawArrows(chapter, parents, chapterX, chapterY);

        layer.add(group);
    }

    function drawChapter(chapter) {
        var chapterBlock = new Konva.Rect({
            x: 0,// regarding the group coordinate 
            y: 0,
            width: ChapterSize.Width,
            height: ChapterSize.Height,
            //fill: "#0f0",
            stroke: "#000",
            strokeWidth: 1,
            shadowColor: '#000',
            shadowBlur: 2,
            shadowOffset: { x: 2, y: 2 },
            shadowOpacity: 1,
        });

        chapterBlock.chapter = chapter;

        //chapterBlock.on('mouseover', function (chap) {
        //    var box = chap.target;
        //    box.shadowColor('red');
        //    box.draw();
        //});
        //chapterBlock.on('mouseout', function (obj) {
        //    var chapter = this.chapter;
        //    var x = obj.target.attrs.x;
        //    var y = obj.target.attrs.y;
        //    var box = obj.target;
        //    box.remove();
        //    drawChapter(chapter, x, y);
        //    stage.draw();
        //});

        return chapterBlock;
        //layer.add(chapterBlock);
    }

    function drawAddChapterButoon(chapter) {
        var addBlock = new Konva.Ellipse({
            x: ChapterSize.Width / 2 - AddButtonSize.Radius / 2,
            y: ChapterSize.Height - AddButtonSize.Radius - AddButtonSize.Padding,
            radius: {
                x: AddButtonSize.Radius,
                y: AddButtonSize.Radius
            },
            fill: "#0f0",
        });
        addBlock.relatedAdd = {};
        addBlock.relatedAdd.chapter = chapter;

        addBlock.on("click", onAddChapterClick);
        cursorPointerHelper(addBlock);
        return addBlock;
    }

    function drawEditChapterButoon(chapter) {
        var editButton = new Konva.Ellipse({
            x: ChapterSize.Width / 2 - AddButtonSize.Radius / 2 - AddButtonSize.Radius - AddButtonSize.Padding * 3,
            y: ChapterSize.Height - AddButtonSize.Radius - AddButtonSize.Padding,
            radius: {
                x: AddButtonSize.Radius,
                y: AddButtonSize.Radius
            },
            fill: "#FFA500",
        });

        editButton.relatedEdit = {};
        editButton.relatedEdit.chapter = chapter;

        editButton.on("click", onEditChapterClick);
        cursorPointerHelper(editButton);
        return editButton;
    }

    function drawRemoveChapterButoon(chapter) {
        var removeBlock = new Konva.Ellipse({
            x: ChapterSize.Width / 2 - AddButtonSize.Radius / 2 + AddButtonSize.Radius + AddButtonSize.Padding * 3,
            y: ChapterSize.Height - AddButtonSize.Radius - AddButtonSize.Padding,
            radius: {
                x: AddButtonSize.Radius,
                y: AddButtonSize.Radius
            },
            fill: "#f00",
        });
        removeBlock.relatedRemove = {};
        removeBlock.relatedRemove.chapter = chapter;

        removeBlock.on("click", onRemoveChapterClick);
        cursorPointerHelper(removeBlock);
        return removeBlock;
    }

    function drawText(chapter) {
        var text = chapter.Name;
        var fontSize = FontSize;// * scale;
        var textItem = new Konva.Text({
            x: 3,// regarding the group coordinate 
            y: 3,
            width: ChapterSize.Width - ChapterSize.Padding,
            fontSize: fontSize,
            fontFamily: "sans-serif",
            text: text,
            fill: "#0aa",
            align: 'center'

        });
        return textItem;
    }

    function drawArrows(chapter, parents, chapterX, chapterY) {
        for (var i = 0; i < parents.length; i++) {
            var parent = parents[i];
            drawArrow(parent.attrs.x, parent.attrs.y, chapterX, chapterY, parent.chapter.Id, chapter.Id);
        }
    }

    function drawArrow(parentX, parentY, chapterX, chapterY, parentId, chapterId) {
        var updatedChapterX = chapterX;
        var updatedChapterY = chapterY;

        var specialArrow = Math.abs(parentY - chapterY) > BlockSize.Height && parentX === updatedChapterX;

        if (parentX === updatedChapterX) {
            parentX += ChapterSize.Width / 2;
            parentY += ChapterSize.Height;
            updatedChapterX += ChapterSize.Width / 2;
        } else {
            parentX += ChapterSize.Width / 2;
            parentY += ChapterSize.Height;
            updatedChapterX += ChapterSize.Width / 2;
        }

        var points = [];

        if (specialArrow) {
            var point1X = parent.attrs.x + ChapterSize.Width;
            var point1Y = parent.attrs.y + ChapterSize.Height * 2 / 3;
            points.push(point1X);
            points.push(point1Y);

            points.push(point1X + ChapterSize.Padding / 2);
            points.push(point1Y);

            points.push(point1X + ChapterSize.Padding / 2);
            points.push(updatedChapterY + ChapterSize.Height / 3);

            points.push(point1X);
            points.push(updatedChapterY + ChapterSize.Height / 3);

        } else {
            points.push(parentX);
            points.push(parentY);

            points.push(parentX);
            points.push(updatedChapterY - ChapterSize.Padding / 2);

            points.push(updatedChapterX);
            points.push(updatedChapterY - ChapterSize.Padding / 2);

            points.push(updatedChapterX);
            points.push(updatedChapterY);
        }

        var arrow = new Konva.Arrow({
            points: points,
            stroke: "#0aa",
            strokeWidth: 2,
            lineCap: "round",
            fill: 'black',
            pointerLength: 8,
            pointerWidth: 12,
        });
        arrow.twoSides = {
            parentId: parentId,
            childId: chapterId
        };
        layer.add(arrow);
    }

    function drawDirectArrows(chapter, parents, chapterX, chapterY) {
        for (var i = 0; i < parents.length; i++) {
            var parent = parents[i];
            drawDirectArrow(parent.attrs.x, parent.attrs.y, chapterX, chapterY, parent.chapter.Id, chapter.Id);
        }
    }

    function drawDirectArrow(parentX, parentY, chapterX, chapterY, parentId, chapterId) {
        var points = [];

        points.push(parentX);
        points.push(parentY);

        points.push(chapterX);
        points.push(chapterY);

        var arrow = new Konva.Arrow({
            points: points,
            stroke: "#0aa",
            strokeWidth: 2,
            lineCap: "round",
            fill: 'black',
            pointerLength: 8,
            pointerWidth: 12,
        });
        arrow.twoSides = {
            parentId: parentId,
            childId: chapterId
        };
        layer.add(arrow);
    }

    function cursorPointerHelper(obj) {
        obj.on('mouseenter', function () {
            stage.container().style.cursor = 'pointer';
        });

        obj.on('mouseleave', function () {
            stage.container().style.cursor = 'default';
        });
    }
    /* draw section END */


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
        });
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
            return canvasItem.attrs.y === y && existItemX >= x - BlockSize.Width && existItemX <= x + BlockSize.Width;
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