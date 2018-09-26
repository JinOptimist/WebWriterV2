var drawShapes = (function () {
    console.log('drawShapes is load');
    var BlockSize = { Width: Const.ChapterSize.Width + Const.ChapterSize.Padding, Height: Const.ChapterSize.Height + Const.ChapterSize.Padding };

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
        var imageObj = new Image();
        imageObj.src = getSrcByChapterState(state);
        imageObj.onload = function () {
            reloadLayer();
        }
        var imageItem = new Konva.Image({
            x: Const.ButtonSize.Padding,
            y: Const.ButtonSize.Padding,
            image: imageObj,
            width: Const.ButtonSize.Radius,
            height: Const.ButtonSize.Radius
        });
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