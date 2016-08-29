
var EventGraph = (function () {
    function drawGraph(events, domElementId, width, height) {
        if (!events || !domElementId)
            return;
        var graph = new Graph();
        var event, i;
        for (i = 0; i < events.length; i++) {
            event = events[i];
            graph.addNode(event.Id, { label: event.Name });
        }

        for (i = 0; i < events.length; i++) {
            event = events[i];
            if (event.ParentEvents == null)
                continue;
            for (var j = 0; j < event.ParentEvents.length; j++) {
                var parent = event.ParentEvents[j];
                graph.addEdge(parent.Id, event.Id, {
                    fill: event.ProgressChanging > 0 ? "green" : "red",
                    label: 'Effective: ' + event.ProgressChanging
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