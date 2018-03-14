var bookMap = (function () {
    console.log('bookMap is load');
    var BlockSize = { Width: 60, Height: 50 };
    var ChapterSize = { Width: 40, Height: 30, Padding: 10 };
    var scale = 1.0
    var canvas = {};

    var levels = [];

    function onChapterBlockClick(obj) {
        console.log('obj - ' + this.chapter.Name);
    }

    function drawChapterBlock(x, y, chapter) {
        var centerX = canvas.width / 2;
        var chapterX = x * BlockSize.Width + ChapterSize.Padding + centerX;
        var chapterY = y * BlockSize.Height + ChapterSize.Padding;
        var chapterBlock = canvas.display.rectangle({
            x: chapterX,
            y: chapterY,
            width: ChapterSize.Width,
            height: ChapterSize.Height,
            //fill: "#0f0",
            stroke: "inside 1px #f0f"
        });
        chapterBlock.chapter = chapter;

        chapterBlock.bind("click", onChapterBlockClick);
        canvas.addChild(chapterBlock);

        var text = chapter.Id + '*' + chapter.Weight;

        var fontSize = 10;// * scale;
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
    
    function start(chapters, newScale) {
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

    function draw(chapters, newScale) {
        if (newScale) {
            scale = newScale;
        }
        canvas.canvas.scale(scale, scale);
        canvas.draw.clear(false);

        levels = [];
        for (var i = 0; i < chapters.length; i++) {
            var chapter = chapters[i];
            var depth = chapter.Level;
            if (depth == 0) {
                //chapters without parent are ignored
                continue;
            }
            var level = levels[depth];
            if (!level) {
                levels[depth] = [];
            }
            levels[depth].push(chapter);
        }

        for (var y = 1; y < levels.length; y++) {
            var level = levels[y];
            for (var i = 0; i < level.length; i++) {
                var x = i - (level.length + 1) / 2;
                var chapter = level[i];
                drawChapterBlock(x, y, chapter);
            }
        }
    }

    
    return {
        start: start,
        redraw: redraw
    };
})();