import allQuestions from "./data/allQuestionsFiltered.json" with {type: "json"};

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
export async function seed(knex) {
  const BATCH_NUMBER = 600;
  const res = allQuestions.map((q) => ({
    ...q,
    topicTags: JSON.stringify(q.topicTags),
  }));
  await knex("all_questions").del();
  for (let i = 0; i < res.length; i += BATCH_NUMBER) {
    await knex("all_questions").insert(res.slice(i, i + BATCH_NUMBER));
  }
}
