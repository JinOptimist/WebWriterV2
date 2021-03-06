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
        if (!parent.LinksFromThisChapter || parent.LinksFromThisChapter.length === 0)
            return false;
        if (parent.Level > child.Level)
            return false;
        return parent.LinksFromThisChapter.filter(link => link.ToId === child.Id).length > 0;
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

    function calcState(current, selected) {
        //drawShapes.chapterStateType
        if (!selected) {
            return drawShapes.chapterStateType.Initial;
        } else if (current.Id === selected.Id) {
            return drawShapes.chapterStateType.Selected;
        } else if (current.Id < 0) {
            return drawShapes.chapterStateType.FakeNew;
        } else if (chaptersAreLinked(selected, current)) {
            return drawShapes.chapterStateType.Child;
        } else if (chaptersAreLinked(current, selected)) {
            return drawShapes.chapterStateType.Parent;
        //TODO: replace hot fix to normal code
        //} else if (selected.ParentsIds.indexOf(current.Id) > -1) {
        } else if (selected.parents.map(x => x.Id).indexOf(current.Id) > -1) {
            return drawShapes.chapterStateType.ForbiddenToLink;
        } else {
            return drawShapes.chapterStateType.AvailableToLink;
        }
    }

    function validateChapter(chapter) {
        if (!chapter) {
            return false;
        }

        if (chapter.LinksFromThisChapter.length > 1) {
            var linkWithoutText = chapter.LinksFromThisChapter.find(x => !x.Text);
            if (linkWithoutText) {
                return false;
            }
        }
        
        return true;
    }

    

    return {
        splitByLevels: splitByLevels,
        chaptersAreLinked: chaptersAreLinked,
        groupByParent: groupByParent,
        calcState: calcState,
        validateChapter: validateChapter
    };
})();