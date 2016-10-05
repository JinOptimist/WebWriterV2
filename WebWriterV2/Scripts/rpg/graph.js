
var EventGraph = (function () {
    function drawGraph(events, domElementId, width, height) {
        if (!events || !domElementId)
            return;
        document.getElementById(domElementId).innerHTML = "";

        var graph = new Graph();
        var event, i;
        for (i = 0; i < events.length; i++) {
            event = events[i];
            graph.addNode(event.Id, {
                label: event.Name,
            });
        }

        for (i = 0; i < events.length; i++) {
            event = events[i];
            if (event.ChildrenEvents == null)
                continue;
            for (var j = 0; j < event.ChildrenEvents.length; j++) {
                var child = event.ChildrenEvents[j];
                var color = event.ProgressChanging > 0 ? "green" : "red";
                graph.addEdge(event.Id, child.Id, {
                    fill: color,
                    stroke: color,
                    label: 'Effective: ' + event.ProgressChanging,
                    directed: true,
                });
            }
        }

        // stroke: "#fff", fill: "#5a5", // цвета ребра
        // directed: true, // ориентированное ребро
        // label: 'l1'

        // вычисляем расположение вершин перед выводом
        var layouter = new Graph.Layout.Spring(graph);
        layouter.layout();

        // выводим граф
        var renderer = new Graph.Renderer.Raphael(domElementId, graph, width, height);
        renderer.draw();
    }

    return {
        drawGraph: drawGraph
    }
})();