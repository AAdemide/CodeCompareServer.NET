import neetcode150 from "./data/neetcode150_collection_seed.json" with {type: "json"};
import collections from "./data/collections.json" with {type: "json"};

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */

export async function seed(knex) {
  let neetcode150_collection = neetcode150.map( async (q, i) => ({
    ...q,
    id: await knex('all_question').select('id').where({titleSlug: q.question_slug})[0],
    question_tags: JSON.stringify(q.question_tags),
    collections_id: collections[0].id
  }));

  await knex("collection_questions").del();
  await knex("collection_questions").insert(neetcode150_collection);
}
