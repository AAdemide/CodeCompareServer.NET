import fs from "fs";
import axios from "axios";

const slugs = (fs.readFileSync("./leetcode_slugs.txt", "utf8")).split("\n");
let resJson = [];

for (const i of slugs) {
    const {data: res} = await axios(`http://localhost:3000/select?titleSlug=${i}`);

    resJson.push({
        leetcode_id: res.questionId,
        question_name: res.questionTitle,
        question_slug: res.titleSlug,
        question_difficulty: res.difficulty,
        question_tags: res.topicTags
    });

}
fs.writeFileSync("neetcode150_collection_seed.json", JSON.stringify(resJson, null, 2))