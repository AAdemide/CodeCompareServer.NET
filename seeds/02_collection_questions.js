import neetcode150 from "./data/neetcode150_collection_seed.json" with {type: "json"};
import collections from "./data/collections.json" with {type: "json"};

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */

export async function seed(knex) {

  let neetcode150_collection = [];
  for(let i = 0; i < neetcode150.length; i++) {
    const q = neetcode150[i]
    const {id: question_id} = (await knex('all_questions').select('id').where({titleSlug: q.question_slug}))[0];
    const {id: collections_id} = (await knex('collections').select('id').where({collection_name: collections[0]["collection_name"]}))[0];
    neetcode150_collection.push({
    ...q,
    question_id: question_id,
    question_tags: JSON.stringify(q.question_tags),
    collections_id: collections_id
    })
  }

  console.log(neetcode150_collection.length)

  await knex("collection_questions").del();
  await knex("collection_questions").insert(neetcode150_collection);
}
