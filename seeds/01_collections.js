import collections from "./data/collections.json" with {type: "json"};

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */

export async function seed(knex) {
  await knex("collections").del();
  await knex("collections").insert(collections);
}
