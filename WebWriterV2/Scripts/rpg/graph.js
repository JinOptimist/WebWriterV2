
var EventGraph = (function () {
    function drawGraph(events, domElementId, width, height) {
        if (!events || !domElementId)
            return;
        document.getElementById(domElementId).innerHTML = "";

        var graph = new Graph();
        var event, i;
        // ������ ������� �����
        for (i = 0; i < events.length; i++) {
            event = events[i];
            graph.addNode(event.Id, {
                label: event.Name,
            });
        }

        // ������ ����� �����
        for (i = 0; i < events.length; i++) {
            event = events[i];
            if (event.LinksFromThisEvent == null)
                continue;
            for (var j = 0; j < event.LinksFromThisEvent.length; j++) {
                var linkFromThisEvent = event.LinksFromThisEvent[j];
                graph.addEdge(linkFromThisEvent.FromId, linkFromThisEvent.ToId, {
                    fill: "#a2a",
                    stroke: "#c2c",
                    //label: 'Effective: ' + event.ProgressChanging,
                    directed: true,
                });
            }
        }

        // stroke: "#fff", fill: "#5a5", // ����� �����
        // directed: true, // ��������������� �����
        // label: 'l1'

        // ��������� ������������ ������ ����� �������
        var layouter = new Graph.Layout.Spring(graph);
        layouter.layout();

        // ������� ����
        var renderer = new Graph.Renderer.Raphael(domElementId, graph, width, height);
        renderer.draw();
    }

    return {
        drawGraph: drawGraph
    }
})();