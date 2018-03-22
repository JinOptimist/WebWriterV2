var bookMap = (function () {
    console.log('bookMap is load');
    var ChapterSize = { Width: 100, Height: 40, Padding: 20 };
    var BlockSize = { Width: ChapterSize.Width + ChapterSize.Padding, Height: ChapterSize.Height + ChapterSize.Padding };
    var AddButtonSize = { Radius: 5, Padding: 3 };
    var ArrowSize = 5;
    var FontSize = 12;
    var scale = 1.0
    var canvas = {};
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

        drawChapter(chapter, chapterX, chapterY);

        drawAddChapterButoon(chapter, chapterX, chapterY);
        drawEditChapterButoon(chapter, chapterX, chapterY);
        drawRemoveChapterButoon(chapter, chapterX, chapterY);

        drawText(chapter, chapterX, chapterY);

        drawArrow(chapter, parents, chapterX, chapterY);
    }

    function drawChapter(chapter, chapterX, chapterY) {
        var chapterBlock = canvas.display.rectangle({
            x: chapterX,
            y: chapterY,
            width: ChapterSize.Width,
            height: ChapterSize.Height,
            //fill: "#0f0",
            stroke: "inside 1px #f0f"
        });
        chapterBlock.chapter = chapter;

        chapterBlock.bind("click", onChapterClick);
        canvas.addChild(chapterBlock);
    }

    function drawAddChapterButoon(chapter, chapterX, chapterY) {
        var addBlock = canvas.display.ellipse({
            x: chapterX + ChapterSize.Width / 2 - AddButtonSize.Radius / 2,
            y: chapterY + ChapterSize.Height - AddButtonSize.Radius - AddButtonSize.Padding,
            radius_x: AddButtonSize.Radius,
            radius_y: AddButtonSize.Radius,
            fill: "#0f0",
        });
        addBlock.relatedAdd = {};
        addBlock.relatedAdd.chapter = chapter;

        addBlock.bind("click", onAddChapterClick);
        canvas.addChild(addBlock);
    }

    function drawEditChapterButoon(chapter, chapterX, chapterY) {
        var addBlock = canvas.display.ellipse({
            x: chapterX + ChapterSize.Width / 2 - AddButtonSize.Radius / 2 - AddButtonSize.Radius - AddButtonSize.Padding * 3,
            y: chapterY + ChapterSize.Height - AddButtonSize.Radius - AddButtonSize.Padding,
            radius_x: AddButtonSize.Radius,
            radius_y: AddButtonSize.Radius,
            fill: "#FFA500",
        });

        addBlock.relatedEdit = {};
        addBlock.relatedEdit.chapter = chapter;

        addBlock.bind("click", onEditChapterClick);
        canvas.addChild(addBlock);
    }

    function drawRemoveChapterButoon(chapter, chapterX, chapterY) {
        var addBlock = canvas.display.ellipse({
            x: chapterX + ChapterSize.Width / 2 - AddButtonSize.Radius / 2 + AddButtonSize.Radius + AddButtonSize.Padding * 3,
            y: chapterY + ChapterSize.Height - AddButtonSize.Radius - AddButtonSize.Padding,
            radius_x: AddButtonSize.Radius,
            radius_y: AddButtonSize.Radius,
            fill: "#f00",
        });
        addBlock.relatedRemove = {};
        addBlock.relatedRemove.chapter = chapter;

        addBlock.bind("click", onRemoveChapterClick);
        canvas.addChild(addBlock);
    }

    function drawText(chapter, chapterX, chapterY) {
        //var text = chapter.Id + '*' + chapter.Weight;
        var text = chapter.Name;

        while (canvas.canvas.measureText(text).width > ChapterSize.Width - ChapterSize.Padding) {
            text = text.substr(0, text.length - 1);
        }

        var fontSize = FontSize;// * scale;
        var textItem = canvas.display.text({
            x: chapterX + 3,
            y: chapterY + 3,
            origin: { x: "left", y: "top" },
            font: fontSize + "px sans-serif",
            text: text,
            fill: "#0aa"
        });
        canvas.addChild(textItem);
    }

    function drawArrow(chapter, parents, chapterX, chapterY) {
        for (var i = 0; i < parents.length; i++) {
            var parent = parents[i];
            var updatedChapterX = chapterX;
            var updatedChapterY = chapterY;
            var parentX = parent.x;
            var parentY = parent.y;

            var startFromBottom = Math.abs(parentY - chapterY) > BlockSize.Height;
            var parentOnTheRight = parentX > updatedChapterX;

            if (parentX == updatedChapterX) {
                parentX += ChapterSize.Width / 2;
                parentY += ChapterSize.Height;
                updatedChapterX += ChapterSize.Width / 2;
            } else if (parentOnTheRight) {
                // if parrent on the right
                if (startFromBottom) {
                    parentX += ChapterSize.Width / 2;
                    parentY += ChapterSize.Height;
                    updatedChapterY += ChapterSize.Height / 3;
                    updatedChapterX += ChapterSize.Width;
                } else {
                    parentY += ChapterSize.Height * 2 / 3;
                    updatedChapterX += ChapterSize.Width / 2;
                }
            } else if (!parentOnTheRight) {
                // if parrent on the left
                if (startFromBottom) {
                    parentX += ChapterSize.Width / 2;
                    parentY += ChapterSize.Height;
                    updatedChapterY += ChapterSize.Height / 3;
                } else {
                    parentX += ChapterSize.Width;
                    parentY += ChapterSize.Height * 2 / 3;
                    updatedChapterX += ChapterSize.Width / 2;
                }
            }



            if (startFromBottom) {
                drawLineHelper(parentX, parentY, parentX, updatedChapterY);
                drawLineHelper(parentX, updatedChapterY, updatedChapterX, updatedChapterY);

                var arrowDirection = parentOnTheRight ? 1 : -1;
                drawLineHelper(updatedChapterX, updatedChapterY, updatedChapterX + ArrowSize * arrowDirection, updatedChapterY - ArrowSize);
                drawLineHelper(updatedChapterX, updatedChapterY, updatedChapterX + ArrowSize * arrowDirection, updatedChapterY + ArrowSize);
            } else {
                drawLineHelper(parentX, parentY, updatedChapterX, parentY);
                drawLineHelper(updatedChapterX, parentY, updatedChapterX, updatedChapterY);

                drawLineHelper(updatedChapterX, updatedChapterY, updatedChapterX - ArrowSize, updatedChapterY - ArrowSize);
                drawLineHelper(updatedChapterX, updatedChapterY, updatedChapterX + ArrowSize, updatedChapterY - ArrowSize);
            }


        }
    }

    function drawLineHelper(startX, startY, endX, endY) {
        line = canvas.display.line({
            start: { x: startX, y: startY },
            end: { x: endX, y: endY },
            stroke: "1px #0aa",
            cap: "round"
        });
        canvas.addChild(line);
    }
    /* draw section END */


    function draw(chapters, newScale) {
        if (newScale) {
            scale = newScale;
        }
        canvas.canvas.scale(scale, scale);
        canvas.draw.clear(false);

        splitByLevels(chapters);
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
                    drawChapterBlock(x, y, chapter);
                    currentX += chapter.Weight * 2;
                }
            }

        }
    }

    function splitByLevels(chapters) {
        var maxWeight = 1;
        levels = [];
        for (var i = 0; i < chapters.length; i++) {
            var chapter = chapters[i];
            chapter.parents = findParents(chapters, chapter.Id);
            var depth = chapter.Level;
            if (depth == 0) {
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


        ChapterSize.Width = canvas.canvas.canvas.offsetWidth / (maxWeight + 1);
        BlockSize.Width = ChapterSize.Width + ChapterSize.Padding;
    }

    function findParents(chapters, chapterId) {
        return chapters.filter(function (chapter) {
            return chapterContainsLinkToCurrentChapter(chapter, chapterId);
        });
    }

    function getParrentsCanvasObj(chapterId) {
        return canvas.children.filter(function (canvasItem) {
            if (!canvasItem.chapter)
                return false;
            var chapter = canvasItem.chapter;
            return chapterContainsLinkToCurrentChapter(chapter, chapterId);
        });
    }

    function chapterContainsLinkToCurrentChapter(chapter, chapterId) {
        if (!chapter.LinksFromThisEvent || chapter.LinksFromThisEvent.length == 0)
            return false;
        return chapter.LinksFromThisEvent.filter(link => link.ToId == chapterId).length > 0;
    }

    function getCenterByParents(parents) {
        if (!parents || parents.length < 1)
            return canvas.width / 2 - BlockSize.Width / 2;

        var xCoordinates = parents.map(chapterBlock => chapterBlock.x);

        var min = Math.min.apply(Math, xCoordinates);
        var max = Math.max.apply(Math, xCoordinates);
        var result = (min + max) / 2;
        //if (min != max) {
        //    result -= BlockSize.Width / 2;
        //}
        return result - ChapterSize.Padding;
    }

    function groupByParent(chapters) {
        if (chapters.length == 1)
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
            if (ch.parents[0].Id != groupByParentId) {
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

    /* public functions */
    function start(chapters, newScale, _actions) {
        actions = _actions;
        canvas = oCanvas.create({
            canvas: "#nicePic"
        });

        draw(chapters, newScale);
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