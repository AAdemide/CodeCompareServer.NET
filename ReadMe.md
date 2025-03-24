# CodeCompare
## Installation instructions
Please have docker installed and run the following command:   docker run -p 3000:3000 alfaarghya/alfa-leetcode-api:2.0.1
## Overview

The app lets you solve coding problems and compare your solutions with AI-generated ones. After submitting your code, the AI analyzes it and provides detailed feedback on improvements. It includes a built-in problem collection, particularly from NeetCode.io, and an optional timer for interview simulations.

A valuable addition would be support for multiple developers, allowing solutions to be ranked by efficiency. This fosters peer learning and provides a structured way to manage programming challenge submissions.

The app is ideal for those looking to improve their LeetCode/whiteboarding skills by offering AI-driven comparisons and analysis in one place.

### Problem Space
The problem space this app addresses involves several key challenges that developers face when preparing for technical interviews:

1. **Lack of comprehensive feedback** - When practicing coding problems independently, developers often have no way to know if their solution is optimal or follows best practices. While platforms like LeetCode provide test cases, they lack detailed feedback on code quality, efficiency, and potential improvements.

2. **Disconnected learning experience** - Currently, developers typically have to use multiple platforms - one for problem-solving, another for comparing different approaches, and perhaps consulting Stack Overflow or GitHub for best practices. This fragmented approach is inefficient.

3. **Limited comparative learning** - Most platforms don't allow developers to easily compare their solutions with alternative approaches. Understanding different solutions to the same problem is crucial for developing problem-solving flexibility.

4. **Isolation in practice** - Technical interview preparation is often a solitary experience, missing the collaborative learning benefits that would come from seeing how peers approach the same problems.

5. **Inefficient feedback loops** - Without immediate, detailed feedback, developers can develop bad coding habits or miss opportunities to improve their approach.

6. **Inaccessible interview simulation** - While timing is critical in interview scenarios, most practice platforms don't integrate realistic interview conditions with comprehensive feedback.

7. **Limited personalization** - Existing platforms rarely adapt to individual learning needs or highlight recurring patterns in a developer's approach that could be improved.


### User Profile

#### Primary Users

##### The Interview Candidate
- **Description**: Software engineers/developers preparing for technical interviews at tech companies.
- **Goals**: 
  - Improve problem-solving skills
  - Practice common coding interview questions
  - Understand optimal solutions and approaches
  - Reduce interview anxiety through practice
- **Pain Points**:
  - Lacks feedback on code quality beyond passing test cases
  - Unsure if their solutions are optimal or follow best practices
  - Limited exposure to alternative approaches
  - Difficulty simulating real interview pressure
  - No personalized guidance on improvement areas

##### The Self-Taught Developer
- **Description**: Individuals learning programming without formal education who need structured practice.
- **Goals**:
  - Build confidence in coding abilities
  - Learn industry-standard approaches to common problems
  - Develop good coding habits early
  - Create portfolio of solved problems
- **Pain Points**:
  - Unsure if self-taught methods are effective
  - Limited access to mentorship and feedback
  - Overwhelmed by number of resources available
  - Difficulty gauging personal progress

##### The CS Student
- **Description**: College students studying computer science who want to supplement their coursework.
- **Goals**:
  - Apply theoretical knowledge to practical problems
  - Prepare for internship/job interviews
  - Reinforce algorithm and data structure concepts
  - Compete with peers in a friendly environment
- **Pain Points**:
  - Gap between academic learning and applied problem-solving
  - Limited practical examples in coursework
  - Need for varied difficulty levels to match current knowledge
  - Desire for peer comparison and collaboration


### Features

#### Core Features

##### Problem Repository
- View problem descriptions with clear requirements and examples
- Select questions from a preset collections in this case neetcode.io

##### Code Editor
- Write solutions in JS
- Run code against example test cases 
- Submit solutions for comprehensive evaluation

##### AI Solution Analysis
- Receive detailed feedback on submitted code
- Get performance analysis (time and space complexity)
- View suggestions for code optimization and best practices
- Identify potential edge cases not handled in the solution
- Highlight coding style improvements and readability enhancements

##### AI-Generated Solutions
- Compare personal solution with AI-generated alternatives
- Access explanations for each AI solution approach
- See step-by-step breakdowns of solution strategies

##### Interview Simulation
- Optional timer for realistic interview pressure



## Implementation

### Tech Stack

#### Front-End
- ReactJS
- CSS / Sass / MaterialUI
- axios
- CodeMirror IDE or Monaco Editor

#### Back End
- NodeJs
- Express
- axios
- knex.js
- AI API ...still deciding


### APIs
- AI API ...still deciding

### Sitemap

- Homepage
- List of my solutions and related questions

### Mockups

![alt text](image.png)
![alt text](image-1.png)

### Data

- A table of questions in the neetcode 150 collection
- A table of my solved questions
- A table of my solutions

### Endpoints

- get collection
- get all solved questions and their related solutions


## Roadmap

- Final UI clean up and testing
- Implement UI for all routes
- Implement Backend
- Create a database of questions
- Integrate AI to analyze code
- Integrate editor and implement running of code
---

## Future Implementations
#### Advanced Features

##### Collaborative Learning
- Create or join coding rooms with peers
- View real-time solution submissions from group members
- Compare performance metrics among participants
- Comment on and discuss different approaches
- Ranking system based on solution efficiency and correctness
