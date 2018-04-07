var drawShapes = (function () {
    console.log('drawShapes is load');
    var BlockSize = { Width: Const.ChapterSize.Width + Const.ChapterSize.Padding, Height: Const.ChapterSize.Height + Const.ChapterSize.Padding };

    var shapeLogicType = {
        ChapterBorder: 'ChapterBorder',
        ChapterGroup: 'ChapterGroup',
        ChapterButton: 'ChapterButton',
        Arrow: 'Arrow',
    };

    function drawChapter(chapter, isHighlight) {
        var chapterBlock = new Konva.Rect({
            x: 0,// regarding the group coordinate 
            y: 0,
            width: Const.ChapterSize.Width,
            height: Const.ChapterSize.Height,
            fill: "#fff",
            stroke: "#000",
            strokeWidth: 1,
            shadowColor: isHighlight ? '#F00' : '#000',
            shadowBlur: 2,
            shadowOffset: { x: 2, y: 2 },
            shadowOpacity: 1,
        });

        chapterBlock.chapter = chapter;
        chapterBlock.logicType = shapeLogicType.ChapterBorder;

        //chapterBlock.on('mouseover', function (chap) {
        //    var box = chap.target;
        //    box.shadowColor('red');
        //    box.draw();
        //});
        //chapterBlock.on('mouseout', function (chap) {
        //    //var chapter = this.chapter;
        //    //var x = obj.target.attrs.x;
        //    //var y = obj.target.attrs.y;
        //    //var box = obj.target;
        //    //box.remove();
        //    //drawChapter(chapter);
        //    //box.parent.draw();
        //    var box = chap.target;
        //    box.shadowColor('#fff');
        //    box.draw();
        //});

        return chapterBlock;
    }

    //function drawAddChapterButton(chapter, onAddChapterClick) {
    //    var addBlock = new Konva.Ellipse({
    //        x: Const.ChapterSize.Width / 2 - Const.AddButtonSize.Radius / 2,
    //        y: Const.ChapterSize.Height - Const.AddButtonSize.Radius - Const.AddButtonSize.Padding,
    //        radius: {
    //            x: Const.AddButtonSize.Radius,
    //            y: Const.AddButtonSize.Radius
    //        },
    //        fill: "#0f0",
    //    });
    //    addBlock.relatedAdd = {};
    //    addBlock.relatedAdd.chapter = chapter;

    //    addBlock.logicType = shapeLogicType.ChapterButton;
    //    addBlock.on("click", onAddChapterClick);
    //    return addBlock;
    //}

    function drawMainButton(chapter, onMainButtonClick, reloadLayer) {
        var imageObj = new Image();
        imageObj.src = '/Content/icon/map-add.png';
        imageObj.onload = function () {
            reloadLayer();
        }
        var addBlock = new Konva.Image({
            x: 30,
            y: (Const.ChapterSize.Height - 40) / 2,
            image: imageObj,
            width: Const.ButtonSize.Radius,
            height: Const.ButtonSize.Radius

        });
        addBlock.relatedAdd = {};
        addBlock.relatedAdd.chapter = chapter;

        addBlock.logicType = shapeLogicType.ChapterButton;
        addBlock.on("click", onMainButtonClick);
        return addBlock;
    }

    //function drawEditChapterButton(chapter, onEditChapterClick) {
    //    var editButton = new Konva.Ellipse({
    //        x: Const.ChapterSize.Width / 2 - Const.AddButtonSize.Radius / 2 - Const.AddButtonSize.Radius - Const.AddButtonSize.Padding * 3,
    //        y: Const.ChapterSize.Height - Const.AddButtonSize.Radius - Const.AddButtonSize.Padding,
    //        radius: {
    //            x: Const.AddButtonSize.Radius,
    //            y: Const.AddButtonSize.Radius
    //        },
    //        fill: "#FFA500",
    //    });

    //    editButton.relatedEdit = {};
    //    editButton.relatedEdit.chapter = chapter;

    //    editButton.logicType = shapeLogicType.ChapterButton;
    //    editButton.on("click", onEditChapterClick);
    //    return editButton;
    //}

    //function drawRemoveChapterButton(chapter, onRemoveChapterClick) {
    //    var removeBlock = new Konva.Ellipse({
    //        x: Const.ChapterSize.Width / 2 - Const.AddButtonSize.Radius / 2 + Const.AddButtonSize.Radius + Const.AddButtonSize.Padding * 3,
    //        y: Const.ChapterSize.Height - Const.AddButtonSize.Radius - Const.AddButtonSize.Padding,
    //        radius: {
    //            x: Const.AddButtonSize.Radius,
    //            y: Const.AddButtonSize.Radius
    //        },
    //        fill: "#f00",
    //    });
    //    removeBlock.relatedRemove = {};
    //    removeBlock.relatedRemove.chapter = chapter;

    //    removeBlock.logicType = shapeLogicType.ChapterButton;
    //    removeBlock.on("click", onRemoveChapterClick);
    //    return removeBlock;
    //}

    /* for future */
    //function drawRemoveChapterImage(chapter, onRemoveChapterClick) {
    //    var imageObj = new Image();
    //    imageObj.src = '/Content/icon/close.png';
    //    var removeBlock = new Konva.Image({
    //        x: Const.ChapterSize.Width / 2 - Const.AddButtonSize.Radius / 2 + Const.AddButtonSize.Radius + Const.AddButtonSize.Padding * 3,
    //        y: Const.ChapterSize.Height - Const.AddButtonSize.Radius - Const.AddButtonSize.Padding,
    //        image: imageObj
    //    });
    //    removeBlock.relatedRemove = {};
    //    removeBlock.relatedRemove.chapter = chapter;

    //    removeBlock.logicType = shapeLogicType.ChapterButton;
    //    removeBlock.on("click", onRemoveChapterClick);
    //    return removeBlock;
    //}

    //function drawAddLinkButton(chapter, onAddLinkClick) {
    //    var addLink = new Konva.Ellipse({
    //        x: Const.ChapterSize.Width / 2 - Const.AddButtonSize.Radius / 2 + (Const.AddButtonSize.Radius + Const.AddButtonSize.Padding) * 3 + Const.AddButtonSize.Padding,
    //        y: Const.ChapterSize.Height - Const.AddButtonSize.Radius - Const.AddButtonSize.Padding,
    //        radius: {
    //            x: Const.AddButtonSize.Radius,
    //            y: Const.AddButtonSize.Radius
    //        },
    //        fill: "#F0F",
    //    });

    //    addLink.chapter = chapter;

    //    addLink.logicType = shapeLogicType.ChapterButton;
    //    addLink.on("click", onAddLinkClick);
    //    return addLink;
    //}

    function drawText(chapter) {
        var text = chapter.Name;
        var textItem = new Konva.Text({
            x: Const.ButtonSize.Padding * 2 + Const.ButtonSize.Radius,// regarding the group coordinate 
            y: Const.ButtonSize.Padding,
            width: Const.ChapterSize.Width - Const.ChapterSize.Padding - 2 * Const.ButtonSize.Padding - Const.ButtonSize.Radius,
            FontSize: Const.FontSize,
            fontFamily: "Lato",
            text: text,
            fill: Const.FontColor,
            align: 'left'
        });
        return textItem;
    }

    function drawArrows(child, parents, onArrowClick) {
        var arrows = [];
        for (var i = 0; i < parents.length; i++) {
            var parent = parents[i];
            var arrow = drawArrow(parent, child, onArrowClick);
            arrows.push(arrow);
        }
        return arrows;
    }

    function drawArrow(parent, child, onArrowClick, isHighlight) {
        var parentIndex = fillDrawnForParent(parent, child);
        var childIndex = fillDrawnForChild(parent, child);

        var links = child.chapter.LinksToThisEvent;
        var linkId = links.find(x=>x.FromId == parent.chapter.Id).Id;

        var parentShift = parentIndex - (parent.chapter.LinksFromThisEvent.length - 1) / 2;
        var childShift = childIndex - (child.chapter.LinksToThisEvent.length - 1) / 2;

        var parentX = parent.attrs.x + Const.ChapterSize.Width / 2;
        var parentY = parent.attrs.y + Const.ChapterSize.Height;

        var childX = child.attrs.x + Const.ChapterSize.Width / 2;
        var childY = child.attrs.y;

        var specialArrow = Math.abs(parentY - childY) > BlockSize.Height && parentX === childX;

        parentX += (parentShift * Const.ArrowShiftX);
        childX += (childShift * Const.ArrowShiftX);

        var points = [];

        if (specialArrow) {
            //   |__lv1__| -->|
            //   |__lv2__|    |
            //   |__lv3__|<-- |

            var point1X = parentX + Const.ChapterSize.Width / 2;
            var point1Y = parentY - Const.ChapterSize.Height / 3;
            points.push(point1X);
            points.push(point1Y);

            points.push(point1X + Const.ChapterSize.Padding / 2);
            points.push(point1Y);

            points.push(point1X + Const.ChapterSize.Padding / 2);
            points.push(childY + Const.ChapterSize.Height / 3);

            points.push(point1X);
            points.push(childY + Const.ChapterSize.Height / 3);

        } else {
            points.push(parentX);
            points.push(parentY);

            points.push(parentX);
            points.push(childY - Const.ChapterSize.Padding / 2 + childShift * Const.ArrowShiftY);

            points.push(childX);
            points.push(childY - Const.ChapterSize.Padding / 2 + childShift * Const.ArrowShiftY);

            points.push(childX);
            points.push(childY);
        }

        var arrow = new Konva.Arrow({
            points: points,
            stroke: isHighlight ? Const.ArrowHighlightColor : Const.ArrowColor,
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
        arrow.linkId = linkId;
        arrow.isHighlight = isHighlight;
        arrow.logicType = shapeLogicType.Arrow;

        arrow.on('click', onArrowClick);

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
        arrow.linkId = linkId;
        arrow.isHighlight = isHighlight;
        arrow.logicType = shapeLogicType.Arrow;

        return arrow;
    }

    function fillDrawnForParent(parent, child) {
         for (var i = 0; i < parent.drawnChildren.length + 1; i++) {
            if (!parent.drawnChildren[i]) {
                parent.drawnChildren[i] = child.chapter.Id;
                return i;
            }
        }
    }

    function fillDrawnForChild(parent, child) {
        for (var i = 0; i < child.drawnParents.length + 1; i++) {
            if (!child.drawnParents[i]) {
                child.drawnParents[i] = parent.chapter.Id;
                return i;
            }
        }
    }

    return {
        //drawAddChapterButton: drawAddChapterButton,
        //drawEditChapterButton: drawEditChapterButton,
        //drawRemoveChapterButton: drawRemoveChapterButton,
        //drawRemoveChapterImage: drawRemoveChapterImage,
        //drawAddLinkButton: drawAddLinkButton,
        drawChapter: drawChapter,
        drawMainButton: drawMainButton,
        drawText: drawText,
        drawArrows: drawArrows,
        drawArrow: drawArrow,
        drawDirectArrows: drawDirectArrows,
        drawDirectArrow: drawDirectArrow,
        shapeLogicType: shapeLogicType
    };
})();