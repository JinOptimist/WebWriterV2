var drawShapes = (function () {
    console.log('drawShapes is load');
    var ChapterSize = Const.ChapterSize;
    var BlockSize = { Width: ChapterSize.Width + ChapterSize.Padding, Height: ChapterSize.Height + ChapterSize.Padding };
    var AddButtonSize = Const.AddButtonSize;
    var FontSize = Const.FontSize;

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
    }

    function drawAddChapterButoon(chapter, onAddChapterClick, cursorPointerHelper) {
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

    function drawEditChapterButoon(chapter, onEditChapterClick, cursorPointerHelper) {
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

    function drawRemoveChapterButoon(chapter, onRemoveChapterClick, cursorPointerHelper) {
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

    function drawArrows(child, parents) {
        var arrows = [];
        for (var i = 0; i < parents.length; i++) {
            var parent = parents[i];
            var arrow = drawArrow(parent, child);
            arrows.push(arrow);
        }
        return arrows;
    }

    function drawArrow(parent, child) {
        parent.drawChildCount++;
        child.drawParentCount++;

        var parentShift = parent.drawChildCount - (parent.chapter.LinksFromThisEvent.length - 1) / 2;
        var childShift = child.drawParentCount - (child.chapter.LinksToThisEvent.length - 1) / 2;

        var parentX = parent.attrs.x + ChapterSize.Width / 2;
        var parentY = parent.attrs.y + ChapterSize.Height;

        var childX = child.attrs.x + ChapterSize.Width / 2;
        var childY = child.attrs.y;

        var specialArrow = Math.abs(parentY - childY) > BlockSize.Height && parentX === childX;

        parentX += (parentShift * 5);
        childX += (childShift * 15);

        var points = [];

        if (specialArrow) {
            //   |__lv1__| -->|
            //   |__lv2__|    |
            //   |__lv3__|<-- |

            var point1X = parentX + ChapterSize.Width / 2;
            var point1Y = parentY - ChapterSize.Height / 3;
            points.push(point1X);
            points.push(point1Y);

            points.push(point1X + ChapterSize.Padding / 2);
            points.push(point1Y);

            points.push(point1X + ChapterSize.Padding / 2);
            points.push(childY + ChapterSize.Height / 3);

            points.push(point1X);
            points.push(childY + ChapterSize.Height / 3);

        } else {
            points.push(parentX);
            points.push(parentY);

            points.push(parentX);
            points.push(childY - ChapterSize.Padding / 2 + childShift * 4);

            points.push(childX);
            points.push(childY - ChapterSize.Padding / 2 + childShift * 4);

            points.push(childX);
            points.push(childY);
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
            parentId: parent.chapter.Id,
            childId: child.chapter.Id
        };
        return arrow;
    }

    function drawDirectArrows(chapterGroup, parents) {
        for (var i = 0; i < parents.length; i++) {
            var parent = parents[i];
            drawDirectArrow(parent, chapterGroup);
        }
    }

    function drawDirectArrow(parent, child) {
        var points = [];

        var parentX = parent.attrs.x;
        var parentY = parent.attrs.y;

        var chapterX = child.attrs.x;
        var chapterY = child.attrs.y;

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
            parentId: parent.chapter.Id,
            childId: child.chapter.Id
        };
        layer.add(arrow);
    }

    return {
        drawChapter: drawChapter,
        drawAddChapterButoon: drawAddChapterButoon,
        drawEditChapterButoon: drawEditChapterButoon,
        drawRemoveChapterButoon: drawRemoveChapterButoon,
        drawText: drawText,
        drawArrows: drawArrows,
        drawArrow: drawArrow,
        drawDirectArrows: drawDirectArrows,
        drawDirectArrow: drawDirectArrow,
    };
})();