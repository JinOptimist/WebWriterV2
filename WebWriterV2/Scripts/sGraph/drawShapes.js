var drawShapes = (function () {
    console.log('drawShapes is load');
    var BlockSize = { Width: Const.ChapterSize.Width + Const.ChapterSize.Padding, Height: Const.ChapterSize.Height + Const.ChapterSize.Padding };
    var Radius = Const.ButtonSize.Radius / 2;

    var useImageForButton = false;

    var shapeLogicType = {
        ChapterBorder: 'ChapterBorder',
        ChapterGroup: 'ChapterGroup',
        ChapterButton: 'ChapterButton',
        Arrow: 'Arrow',
    };

    var chapterStateType = {
        Initial: 'Initial',
        Selected: 'Selected',
        Parent: 'Parent',
        Child: 'Child',
        FakeNew: 'FakeNew',
        AvailableToLink: 'AvailableToLink',
        ForbiddenToLink: 'ForbiddenToLink',
    };

    function drawChapter(chapter, isHighlight) {
        var isFakeChapter = chapter.Id < 0;
        var validChapter = chapterProcessing.validateChapter(chapter);
        var chapterBlock = new Konva.Rect({
            x: 0,// regarding the group coordinate 
            y: 0,
            width: Const.ChapterSize.Width,
            height: Const.ChapterSize.Height,
            fill: "#fff",
            stroke: "#000",
            strokeWidth: 1,
            shadowColor: '#000',
            shadowBlur: 2,
            shadowOffset: { x: 2, y: 2 },
            shadowOpacity: 1,
            cornerRadius: 5
        });

        if (!validChapter) {
            chapterBlock.dash([15, 2]);
            chapterBlock.shadowColor('#F00');
        }

        if (isFakeChapter) {
            chapterBlock.dash([5, 5]);
            chapterBlock.shadowOpacity(0);
            chapterBlock.shadowBlur(0);
            chapterBlock.fill('');
        }

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

    function drawMainButton(chapter, onMainButtonClick, state, reloadLayer) {
        var imageItem = {};

        if (useImageForButton) {
            var imageObj = new Image();
            imageObj.src = getSrcByChapterState(state);
            imageObj.onload = function () {
                reloadLayer();
            }
            imageItem = new Konva.Image({
                x: Const.ButtonSize.Padding,
                y: Const.ButtonSize.Padding,
                image: imageObj,
                width: Const.ButtonSize.Radius,
                height: Const.ButtonSize.Radius
            });
        }  else {
            imageItem = getButtonByState(state);
        }

        imageItem.state = state;
        imageItem.chapter = chapter;
        imageItem.logicType = shapeLogicType.ChapterButton;
        imageItem.on("click", onMainButtonClick);
        return imageItem;
    }

    function drawText(chapter, user) {
        var text = chapter.Name 
            ? chapter.Name +
                 (user.ShowExtendedFunctionality
                     ? ' (' + chapter.StatisticOfVisiting + '%)'
                     : '')
            : '';

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

    function drawArrows(child, parents) {
        var arrows = [];
        for (var i = 0; i < parents.length; i++) {
            var parent = parents[i];
            var arrow = drawArrow(parent, child);
            arrows.push(arrow);
        }
        return arrows;
    }

    function drawArrow(parent, child, isHighlight) {
        var isFakeLine = child.chapter.Id < 0
        var parentIndex = fillDrawnForParent(parent, child);
        var childIndex = fillDrawnForChild(parent, child);

        var links = child.chapter.LinksToThisChapter;
        var link = links.find(x => x.FromId == parent.chapter.Id);
        var hasText = !!link.Text;
        
        var parentShift = parentIndex - (parent.chapter.LinksFromThisChapter.length - 1) / 2;
        var childShift = childIndex - (child.chapter.LinksToThisChapter.length - 1) / 2;

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

            parentX -= (parentShift * Const.ArrowShiftX);
            childX -= (childShift * Const.ArrowShiftX);

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

        var color = hasText ? Const.ArrowHighlightColor : Const.ArrowColor;

        var arrow = new Konva.Arrow({
            points: points,
            stroke: color,// isHighlight ? Const.ArrowHighlightColor : Const.ArrowColor,
            strokeWidth: 2,
            lineCap: "round",
            fill: 'black',
            pointerLength: 8,
            pointerWidth: 12,
        });
        if (isFakeLine) {
            arrow.dash([5, 5]);
        }
        arrow.twoSides = {
            parentId: parent.chapter.Id,
            childId: child.chapter.Id
        };
        arrow.linkId = link.Id;
        arrow.isHighlight = isHighlight;
        arrow.logicType = shapeLogicType.Arrow;

        //arrow.on('click', onArrowClick);

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

    function getSrcByChapterState(state) {
        switch (state) {
            case chapterStateType.Initial:
                return '/Content/icon/map-add.png';
            case chapterStateType.FakeNew:
                return '/Content/icon/map-add-empty.png';
            case chapterStateType.Selected:
                return '/Content/icon/map-remove.png';
            case chapterStateType.AvailableToLink:
                return '/Content/icon/map-link-available.png';
            case chapterStateType.Parent:
                return '/Content/icon/map-link-parent.png';
            case chapterStateType.Child:
                return '/Content/icon/map-link-child.png';
            default:
                return '';
        }
    }

    function getButtonByState(state) {
        switch (state) {
            case chapterStateType.Initial:
                return generateInitialButton(false);
            case chapterStateType.FakeNew:
                return generateInitialButton(true);
            case chapterStateType.Selected:
                return generateRemoveButton();
            case chapterStateType.AvailableToLink:
                return generateAvailableButton();


            case chapterStateType.Parent:
                return generateChildButton();
            case chapterStateType.Child:
                return generateChildButton();
            default:
                return '';
        }
    }

    function generateInitialButton(isFake){
        var group = new Konva.Group({
            x: Const.ButtonSize.Padding + Radius,
            y: Const.ButtonSize.Padding + Radius,
        });

        var lineColor = isFake ? '#71787e' : '#f5f5f5';
        var circleColor = isFake ? '' : '#00aeef';
        var circleStrokeColor = isFake ? '#71787e' : '';

        var circle = new Konva.Circle({
            x: 0,
            y: 0,
            radius: Radius,
            fill: circleColor,
            stroke: circleStrokeColor,
            strokeWidth: 2
        });

        var halfRadius = Radius / 4;
        var lineX = new Konva.Line({
            points: [-halfRadius, 0, halfRadius, 0],
            stroke: lineColor,
            strokeWidth: 3,
            lineCap: 'round',
            lineJoin: 'round'
        });
        var lineY = new Konva.Line({
            points: [0, -halfRadius, 0, halfRadius],
            stroke: lineColor,
            strokeWidth: 3,
            lineCap: 'round',
            lineJoin: 'round'
        });

        group.add(circle);
        group.add(lineX);
        group.add(lineY);
        return group;
    }

    function generateRemoveButton() {
        var group = new Konva.Group({
            x: Const.ButtonSize.Padding + Radius,
            y: Const.ButtonSize.Padding + Radius,
        });

        var circle = new Konva.Circle({
            x: 0,
            y: 0,
            radius: Radius,
            fill: '#f13a30',
        });

        var halfRadius = Radius / 4;
        var lineX = new Konva.Line({
            points: [-halfRadius, -halfRadius, halfRadius, halfRadius],
            stroke: '#fafafa',
            strokeWidth: 3,
            lineCap: 'round',
            lineJoin: 'round'
        });
        var lineY = new Konva.Line({
            points: [halfRadius, -halfRadius, -halfRadius, halfRadius],
            stroke: '#fafafa',
            strokeWidth: 3,
            lineCap: 'round',
            lineJoin: 'round'
        });

        group.add(circle);
        group.add(lineX);
        group.add(lineY);
        return group;
    }

    function generateAvailableButton() {
        var group = new Konva.Group({
            x: Const.ButtonSize.Padding + Radius,
            y: Const.ButtonSize.Padding + Radius,
        });

        var circle = new Konva.Circle({
            x: 0,
            y: 0,
            radius: Radius,
            fill: '#00aeef',
        });

        var x1 = -1 / 2 * Radius;
        var y1 = 1 / 6 * Radius;
        var x2 = -1 / 6 * Radius;
        var y2 = -1 / 6 * Radius;
        var x3 = 1 / 6 * Radius;
        var y3 = 1 / 6 * Radius;
        var x4 = 1 / 2 * Radius;
        var y4 = -1 / 6 * Radius;
        var line = new Konva.Line({
            points: [
                x1, y1,
                x2, y2,
                x3, y3,
                x4, y4],
            stroke: '#f5f5f5',
            strokeWidth: 2,
            lineCap: 'round',
            lineJoin: 'round'
        });

        var lineCircle1 = new Konva.Circle({
            x: x1,
            y: y1,
            radius: 2,
            fill: '#f5f5f5',
        });
        var lineCircle2 = new Konva.Circle({
            x: x2,
            y: y2,
            radius: 2,
            fill: '#f5f5f5',
        });
        var lineCircle3 = new Konva.Circle({
            x: x3,
            y: y3,
            radius: 2,
            fill: '#f5f5f5',
        });
        var lineCircle4 = new Konva.Circle({
            x: x4,
            y: y4,
            radius: 2,
            fill: '#f5f5f5',
        });


        group.add(circle);
        group.add(line);
        group.add(lineCircle1);
        group.add(lineCircle2);
        group.add(lineCircle3);
        group.add(lineCircle4);

        return group;
    }

    function generateChildButton() {
        var group = new Konva.Group({
            x: Const.ButtonSize.Padding + Radius,
            y: Const.ButtonSize.Padding + Radius,
        });

        var circle = new Konva.Circle({
            x: 0,
            y: 0,
            radius: Radius,
            fill: '#f18f1c',
        });

        var radiusDiv2 = Radius / 2;
        var radiusDiv6 = Radius / 6;
        var line1 = new Konva.Line({
            points: [-(Radius - radiusDiv6), 0, -radiusDiv2, 0],
            stroke: '#fafafa',
            strokeWidth: 2,
            lineCap: 'round',
            lineJoin: 'round'
        });
        var line2 = new Konva.Line({
            points: [0, -radiusDiv2,
                -radiusDiv6, 0,
                radiusDiv6, 0,
                0, radiusDiv2],
            stroke: '#fafafa',
            strokeWidth: 2,
            lineCap: 'round',
            lineJoin: 'round'
        });
        var line3 = new Konva.Line({
            points: [Radius - radiusDiv6, 0, radiusDiv2, 0],
            stroke: '#fafafa',
            strokeWidth: 2,
            lineCap: 'round',
            lineJoin: 'round'
        });

        group.add(circle);
        group.add(line1);
        group.add(line2);
        group.add(line3);
        return group;
    }

    return {
        drawChapter: drawChapter,
        drawMainButton: drawMainButton,
        drawText: drawText,
        drawArrows: drawArrows,
        drawArrow: drawArrow,
        shapeLogicType: shapeLogicType,
        chapterStateType: chapterStateType
    };
})();