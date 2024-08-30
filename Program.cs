//*****************************************************************************
//** 2699. Modify Graph Edge Weights  leetcode                               **
//*****************************************************************************


/**
 * Return an array of arrays of size *returnSize.
 * The sizes of the arrays are returned as *returnColumnSizes array.
 * Note: Both returned array and *columnSizes array must be malloced, assume caller calls free().
 */
#define INF 2000000000 // Define constant for infinity value.

// Dijkstra's algorithm to find the shortest path from src to dest.
long long dijkstra(int** edges, int n, int src, int dest, int edgesSize, int* edgesColSize) {
    int i, j;
    long long graph[n][n];
    long long distance[n];
    bool visited[n];
    
    // Initialize all distances to INF and visited to false.
    for (i = 0; i < n; i++) {
        for (j = 0; j < n; j++) {
            graph[i][j] = INF;
        }
        distance[i] = INF;
        visited[i] = false;
    }
    distance[src] = 0;

    // Initialize graph with edges that have non-negative weight.
    for (i = 0; i < edgesSize; i++) {
        int from = edges[i][0], to = edges[i][1], weight = edges[i][2];
        if (weight == -1) {
            continue;
        }
        graph[from][to] = weight;
        graph[to][from] = weight;
    }

    // Applying Dijkstra's algorithm.
    for (i = 0; i < n; i++) {
        int closestUnvisitedNode = -1;
        for (j = 0; j < n; j++) {
            if (!visited[j] && (closestUnvisitedNode == -1 || distance[j] < distance[closestUnvisitedNode])) {
                closestUnvisitedNode = j;
            }
        }
        visited[closestUnvisitedNode] = true;
        for (j = 0; j < n; j++) {
            if (graph[closestUnvisitedNode][j] < INF) {
                distance[j] = (distance[j] < (distance[closestUnvisitedNode] + graph[closestUnvisitedNode][j])) ? distance[j] : (distance[closestUnvisitedNode] + graph[closestUnvisitedNode][j]);
            }
        }
    }

    // Return the shortest distance to the destination.
    return distance[dest];
}

typedef struct {
    int n;           // Number of nodes
    int** edges;     // Edge list
    int source;      // Source node
    int destination; // Destination node
    int target;      // Target distance
} Solution;

// Function to modify graph edges such that the shortest path equals the target distance.
int** modifiedGraphEdges(int n, int** edges, int edgesSize, int* edgesColSize, int source, int destination, int target, int* returnSize, int** returnColumnSizes) {
    int i, j;
    long long shortestDistance = dijkstra(edges, n, source, destination, edgesSize, edgesColSize);
    if (shortestDistance < target) {
        *returnSize = 0;
        return NULL;
    }
    bool isEqualToTarget = (shortestDistance == target);
    for (i = 0; i < edgesSize; i++) {
        if (edges[i][2] > 0) {
            continue;
        }
        if (isEqualToTarget) {
            edges[i][2] = INF;
            continue;
        }
        edges[i][2] = 1;
        shortestDistance = dijkstra(edges, n, source, destination, edgesSize, edgesColSize);
        if (shortestDistance <= target) {
            isEqualToTarget = true;
            edges[i][2] += target - shortestDistance;
        }
    }
    if (isEqualToTarget) {
        *returnSize = edgesSize;
        *returnColumnSizes = edgesColSize;
        return edges;
    } else {
        *returnSize = 0;
        return NULL;
    }
}

