import fs from "fs";
import file1 from "./allQuestions.json" with {type: "json"};

const res = file1.map((q) => {
    const {acRate, title, titleSlug, difficulty, topicTags, hasSolution} = q;
  return {acRate, title, titleSlug, difficulty, topicTags, hasSolution} 
})

fs.writeFileSync("./seeds/data/allQuestionsFiltered.json", JSON.stringify(res, null, 2))
console.log(res)