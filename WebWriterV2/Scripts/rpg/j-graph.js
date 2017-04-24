
var JGraph = (function () {
    var context;
    var width, height;
    var freePoint = {
        x: 10,
        y: 10
    };
    var chapterSize = 100;
    var chapterMargin = 10;
    var lineHeight = 100;
    var textSize = 14;

    var chapters = [];

    var levels = [];

    function drawGraph(_chapters, domElementId, _width, _height) {
        resetVar();
        if (!_chapters || !domElementId)
            return;
        var drawing = document.getElementById(domElementId);
        drawing.innerHTML = "";
        context = drawing.getContext("2d");

        chapters = _chapters;

        width = _width;
        height = _height;

        var rootChapter = chapters.find(function (ch) {
            return ch.LinksToThisEvent.length == 0;
        });

        rootChapter.level = 0;
        calculateLevels(rootChapter);
        
        // Создаём вершины графа
        fillLevels();

        for (i = 0; i < levels.length; i++) {
            var level = levels[i];
            for (j = 0; j < level.length; j++) {
                var chapter = level[j];
                calcPoint(chapter, i, j);
            }
        }

        for (i = 0; i < chapters.length; i++) {
            var chapter = chapters[i];
            drawChapter(chapter);

            for (var j = 0; j < chapter.LinksFromThisEvent.length; j++) {
                var link = chapter.LinksFromThisEvent[j];
                var chapterDestination = getChapterById(link.ToId);
                context.strokeStyle = '#ff0000';
                context.stroke();
                canvasArrow(chapter.x + chapterSize / 2 + getRandomInt(-10, 10),
                    chapter.y + chapterSize / 2 + + getRandomInt(-10, 10),
                    chapterDestination.x + chapterSize / 2 + getRandomInt(-5, 5),
                    chapterDestination.y + getRandomInt(2, 12));
            }
        }

        // Создаём грани графа
        //for (i = 0; i < chapters.length; i++) {
        //    event = chapters[i];
        //    if (event.LinksFromThisEvent == null)
        //        continue;
        //    for (var j = 0; j < event.LinksFromThisEvent.length; j++) {
        //        var linkFromThisEvent = event.LinksFromThisEvent[j];
        //        graph.addEdge(linkFromThisEvent.FromId, linkFromThisEvent.ToId, {
        //            fill: "#a2a",
        //            stroke: "#c2c",
        //            //label: 'Effective: ' + event.ProgressChanging,
        //            directed: true,
        //        });
        //    }
        //}

    }

    function resetVar() {
        context;
        width, height;
        freePoint = {
            x: 10,
            y: 10
        };
        chapterSize = 100;
        chapterMargin = 10;
        lineHeight = 100;
        textSize = 14;

        chapters = [];

        levels = [];
    }

    function fillLevels() {
        var chapter;
        for (i = 0; i < chapters.length; i++) {
            chapter = chapters[i];
            if (!levels[chapter.level])
                levels[chapter.level] = [];
            levels[chapter.level].push(chapter);
        }
    }

    function getChapterById(id) {
        return chapters.find(function (ch) {
            return ch.Id == id;
        })
    }

    function calculateLevels(parentChapter) {
        var currentLevel = parentChapter.level + 1;
        for (var i = 0; i < parentChapter.LinksFromThisEvent.length; i++) {
            var chapter = getChapterById(parentChapter.LinksFromThisEvent[i].ToId);
            if (!chapter.level || chapter.level < currentLevel) {
                chapter.level = currentLevel;
            }
        }
        for (var j = 0; j < parentChapter.LinksFromThisEvent.length; j++) {
            var chapterForCalc = getChapterById(parentChapter.LinksFromThisEvent[j].ToId);
            calculateLevels(chapterForCalc);
        }
        
    }

    function drawChapter(chapter) {
        context.fillStyle = "#ccc";
        context.fillRect(chapter.x, chapter.y, chapterSize, chapterSize);
        
        var textX = chapter.x + chapterMargin;
        var textY = chapter.y + chapterMargin + textSize;
        // draw font in red
        context.fillStyle = "#000";
        context.font = textSize + "px Arial";
        context.fillText(chapter.Name, textX, textY);
    }

    function calcPoint(chapter, levelNumber, chapterNumber) {
        var level = levels[levelNumber]
        var widthCellOnLevel = width / level.length;
        chapter.x = widthCellOnLevel * chapterNumber + (widthCellOnLevel - chapterSize) / 2;
        chapter.y = (lineHeight + chapterMargin) * levelNumber + chapterMargin;        
    }

    function canvasArrow(fromx, fromy, tox, toy) {
        var headlen = 10;   // length of head in pixels
        var angle = Math.atan2(toy - fromy, tox - fromx);
        context.moveTo(fromx, fromy);
        context.lineTo(tox, toy);
        context.lineTo(tox - headlen * Math.cos(angle - Math.PI / 6), toy - headlen * Math.sin(angle - Math.PI / 6));
        context.moveTo(tox, toy);
        context.lineTo(tox - headlen * Math.cos(angle + Math.PI / 6), toy - headlen * Math.sin(angle + Math.PI / 6));
    }

    function getRandomInt(min, max) {
        return Math.floor(Math.random() * (max - min + 1)) + min;
    }

    return {
        drawGraph: drawGraph
    }
})();