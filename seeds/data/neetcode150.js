import fs from "fs";
import axios from "axios";
import path from "path";

const pathname = path.resolve(import.meta.dirname, "neetcode_slugs.txt")
const outPathname = path.resolve(import.meta.dirname, "neetcode150_collection_seed.json")

const slugs = (fs.readFileSync(pathname, "utf8")).split("\n");
let resJson = [];

for (const i of slugs) {
    const {data: res} = await axios(`http://localhost:3000/select?titleSlug=${i}`);

    resJson.push({
        question_name: res.questionTitle,
        question_slug: res.titleSlug,
        question_difficulty: res.difficulty,
        question_tags: res.topicTags
    });

}
fs.writeFileSync(outPathname, JSON.stringify(resJson, null, 2))