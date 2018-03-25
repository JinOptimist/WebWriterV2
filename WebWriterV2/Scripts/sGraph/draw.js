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

        drawChapter(chapter, chapterX, chapterY);

        drawAddChapterButoon(chapter, chapterX, chapterY);
        drawEditChapterButoon(chapter, chapterX, chapterY);
        drawRemoveChapterButoon(chapter, chapterX, chapterY);

        drawText(chapter, chapterX, chapterY);

        drawArrow(chapter, parents, chapterX, chapterY);
    }

    function drawChapter(chapter, chapterX, chapterY) {
        var chapterBlock = new Konva.Rect({
            x: chapterX,
            y: chapterY,
            width: ChapterSize.Width,
            height: ChapterSize.Height,
            //fill: "#0f0",
            stroke: "#000",
            strokeWidth: 1,
            shadowColor: '#000',
            shadowBlur: 2,
            shadowOffset: { x: 2, y: 2 },
            shadowOpacity: 1
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

        layer.add(chapterBlock);
    }

    function drawAddChapterButoon(chapter, chapterX, chapterY) {
        var addBlock = new Konva.Ellipse({
            x: chapterX + ChapterSize.Width / 2 - AddButtonSize.Radius / 2,
            y: chapterY + ChapterSize.Height - AddButtonSize.Radius - AddButtonSize.Padding,
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
        layer.add(addBlock);
    }

    function drawEditChapterButoon(chapter, chapterX, chapterY) {
        var addBlock = new Konva.Ellipse({
            x: chapterX + ChapterSize.Width / 2 - AddButtonSize.Radius / 2 - AddButtonSize.Radius - AddButtonSize.Padding * 3,
            y: chapterY + ChapterSize.Height - AddButtonSize.Radius - AddButtonSize.Padding,
            radius: {
                x: AddButtonSize.Radius,
                y: AddButtonSize.Radius
            },
            fill: "#FFA500",
        });

        addBlock.relatedEdit = {};
        addBlock.relatedEdit.chapter = chapter;

        addBlock.on("click", onEditChapterClick);
        cursorPointerHelper(addBlock);
        layer.add(addBlock);
    }

    function drawRemoveChapterButoon(chapter, chapterX, chapterY) {
        var removeBlock = new Konva.Ellipse({
            x: chapterX + ChapterSize.Width / 2 - AddButtonSize.Radius / 2 + AddButtonSize.Radius + AddButtonSize.Padding * 3,
            y: chapterY + ChapterSize.Height - AddButtonSize.Radius - AddButtonSize.Padding,
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
        layer.add(removeBlock);
    }

    function drawText(chapter, chapterX, chapterY) {
        var text = chapter.Name;
        var fontSize = FontSize;// * scale;
        var textItem = new Konva.Text({
            x: chapterX + 3,
            y: chapterY + 3,
            width: ChapterSize.Width - ChapterSize.Padding,
            fontSize: fontSize,
            fontFamily: "sans-serif",
            text: text,
            fill: "#0aa",
            align: 'center'

        });
        layer.add(textItem);
    }

    function drawArrow(chapter, parents, chapterX, chapterY) {
        for (var i = 0; i < parents.length; i++) {
            var parent = parents[i];
            var updatedChapterX = chapterX;
            var updatedChapterY = chapterY;
            var parentX = parent.attrs.x;
            var parentY = parent.attrs.y;

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
            layer.add(arrow);
        }
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

    function chapterContainsLinkToCurrentChapter(chapter, chapterId) {
        if (!chapter.LinksFromThisEvent || chapter.LinksFromThisEvent.length === 0)
            return false;
        return chapter.LinksFromThisEvent.filter(link => link.ToId === chapterId).length > 0;
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