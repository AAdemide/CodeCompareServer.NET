/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
const up = function (knex) {
  try {
    return knex.schema
      .createTable("users", (table) => {
        table.increments("id").primary();
        table.string("username").notNullable().unique();
        table.string("email").notNullable().unique();
        table.string("password").notNullable();
      })
      .createTable("all_questions", (table) => {
        table.increments("id").primary();
        table.float("acRate").notNullable();
        table.string("title").notNullable();
        table.string("titleSlug").notNullable().unique();
        table.string("difficulty").notNullable();
        table.json("topicTags").notNullable();
        table.boolean("hasSolution").notNullable();
      })
      .createTable("user_questions", (table) => {
        table.increments("id").primary();
        table.string("question_name");
        table.string("question_slug");
        table.string("question_difficulty");
        table.text("unstructured_question_body");
        table.boolean("structured_question").notNullable();
        table.timestamps(true, true);
        table
          .integer("question_id")
          .unsigned()
          .notNullable()
          .references("id")
          .inTable("all_questions")
          .onUpdate("CASCADE")
          .onDelete("CASCADE");
        table
          .integer("user_id")
          .unsigned()
          .notNullable()
          .references("id")
          .inTable("users")
          .onUpdate("CASCADE")
          .onDelete("CASCADE");
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
      .createTable("collections", (table) => {
        table.increments("id").primary();
        table.string("collection_name").notNullable().unique();
      })
      .createTable("collection_questions", (table) => {
        table.integer("question_id").primary();
        table.string("question_name").notNullable();
        table.string("question_slug").notNullable();
        table.string("question_difficulty").notNullable();
        table.json("question_tags");
        table
          .integer("collections_id")
          .unsigned()
          .notNullable()
          .references("id")
          .inTable("collections")
          .onUpdate("CASCADE")
          .onDelete("CASCADE");
      });
  } catch (error) {
    console.error(error);
  }
};

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
const down = function (knex) {
  try {
    return knex.schema
      .dropTable("submissions")
      .dropTable("user_questions")
      .dropTable("collection_questions")
      .dropTable("collections")
      .dropTable("users")
      .dropTable("all_questions");
  } catch (error) {
    console.error(error);
  }
};

export { up, down };
