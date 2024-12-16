The goal is to write a program that connects via the GitHub API to the GitHub - lodash/lodash repository
and gathers a couple of statistics.
The program will output statistics on how often each letter is present in the
JavaScript/TypeScript files of the repository, in decreasing order.

Assumptions:
- Letters from non English alphabet are also counted
- Case-insensitive

RESULTS:  
Key: e, Value: 262122  
Key: t, Value: 211570  
Key: a, Value: 165579  
Key: r, Value: 163042  
Key: n, Value: 140384  
Key: s, Value: 139006  
Key: o, Value: 126620  
Key: i, Value: 119797  
Key: l, Value: 93794  
Key: c, Value: 89012  
Key: u, Value: 88798  
Key: d, Value: 60599  
Key: p, Value: 58761  
Key: h, Value: 53435  
Key: f, Value: 53058  
Key: m, Value: 48137  
Key: b, Value: 39217  
Key: g, Value: 31986  
Key: v, Value: 30079  
Key: y, Value: 27682  
Key: x, Value: 18139  
Key: w, Value: 16380  
Key: k, Value: 12583  
Key: j, Value: 11916  
Key: q, Value: 10412  
Key: z, Value: 3573  
Key: æ, Value: 6  
Key: т, Value: 6  
Key: ä, Value: 4  
Key: τ, Value: 4  
Key: е, Value: 3  
Key: с, Value: 3  
Key: é, Value: 2  
Key: à, Value: 2  
Key: ñ, Value: 2  
Key: W, Value: 1  
Key: Æ, Value: 1  


IMPROVEMENTS:
- Way to many calls to the GitHub API
- Time complexity and space complexity
