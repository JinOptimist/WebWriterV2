var chapterProcessing = (function () {
    console.log('chapterProcessing is load');

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

        return levels;
    }

    function chaptersAreLinked(parent, child) {
        if (!parent.LinksFromThisEvent || parent.LinksFromThisEvent.length === 0)
            return false;
        if (parent.Level > child.Level)
            return false;
        return parent.LinksFromThisEvent.filter(link => link.ToId === child.Id).length > 0;
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

    function findParents(chapters, chapterId) {
        var currentChapter = chapters.find(x=>x.Id === chapterId);
        return chapters.filter(function (chapter) {
            return chaptersAreLinked(chapter, currentChapter);
        });
    }

    return {
        splitByLevels: splitByLevels,
        chaptersAreLinked: chaptersAreLinked,
        groupByParent: groupByParent
    };
})();