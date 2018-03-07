var bookMap = (function () {
    console.log('bookMap is load');
    var blockSize = 30;

    function onChapterBlockClick(obj) {
        console.log('obj - ' + this.name);
    }
    
    function start(chapters) {
        var canvas = oCanvas.create({
            canvas: "#nicePic"
        });

        for (var i = 0 ; i < chapters.length; i++) {
            
            var chapterBlock = canvas.display.rectangle({
                x: i * blockSize + 1,
                y: i * blockSize + 1,
                width: blockSize,
                height: blockSize,
                fill: "#0ff"
            });
            chapterBlock.name = chapters[i].Name;

            chapterBlock.bind("click", onChapterBlockClick);
            canvas.addChild(chapterBlock);
        }
    }

    return {
        start: start
    };
})();