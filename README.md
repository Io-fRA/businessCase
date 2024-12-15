The goal is to write a program that connects via the GitHub API to the GitHub - lodash/lodash repository
and gathers a couple of statistics.
The program will output statistics on how often each letter is present in the
JavaScript/TypeScript files of the repository, in decreasing order.

Assumptions: 
- Only the 26 letters of the latin alphabet will be considered
- Case-insensitive
- .js and .ts files will be considered the same
- Output value will be the total number of occurences of a letter in every .js/.ts file


Road map: 
- Connect to the repo with the GitHub API
- Retrieve a .js file
- Parse the file 
- Use a Map to store occurences of letters
- Recursively parse all
