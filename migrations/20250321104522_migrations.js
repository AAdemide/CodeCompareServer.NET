/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
const up = function (knex) {
  return knex.schema
    .createTable("user_questions", (table) => {
      table.increments("id").primary();
      table.integer("leetcode_id");
      table.string("question_name");
      table.string("question_slug");
      table.string("question_difficulty");
      table.text("unstructured_question_body");
      table.boolean("structured_question").notNullable();
      table.timestamps(true, true);
    })
    .createTable("submissions", (table) => {
      table.increments("id").primary();
      table.text("submission_analyses").notNullable();
      table.timestamp("created_at").defaultTo(knex.fn.now());
      table
        .integer("user_question_id")
        .unsigned()
        .notNullable()
        .references("id")
        .inTable("user_questions")
        .onUpdate("CASCADE")
        .onDelete("CASCADE");
    })
    .createTable("blind_75_collection", (table) => {
      table.integer("leetcode_id").primary();
      table.boolean("done").defaultTo(false);
      table.string("question_name").notNullable();
      table.string("question_slug").notNullable();
      table.string("question_difficulty").notNullable();
      // table.string("question_tags");
    });
};

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
const down = function (knex) {
  return knex.schema
    .dropTable("submissions")
    .dropTable("user_questions")
    .dropTable("blind_75_collection");
};

export { up, down };
