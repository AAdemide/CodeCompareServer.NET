import initKnex from "knex";
import configuration from "../knexfile.js";
const knex = initKnex(configuration);

const index = async (req, res) => {
  try {
    let questions = await knex.select("*").from("user_questions").where({id: req.id});
    res.status(200).json(questions);
  } catch (error) {
    console.error(`Error retrieving questions:${error}`);
    res.status(400).json({ message: `Error retrieving questions:${error}` });
  }
};
//make sure only unique questions are added
const addOne = async (req, res) => {
  try {
    const {
      structured_question,
      leetcode_id,
      question_name,
      question_slug,
      question_difficulty,
      unstructured_question_body,
      user_id,
    } = req.body;
    let newQuestion;
    if (structured_question) {
      if (
        !leetcode_id ||
        !question_name ||
        !question_slug ||
        !question_difficulty ||
        !user_id
      ) {
        return res.status(400).send("All fields are required");
      }
      //do a get here to check if leetcode_id in user_question if so do not add a new question
      let newQuestionId = await knex("user_questions").insert({
        leetcode_id,
        question_name,
        question_slug,
        question_difficulty,
        structured_question,
      });
      newQuestion = await knex("user_questions").where({
        id: newQuestionId[0],
      });
    } else {
      if (!unstructured_question_body || !question_name || !user_id) {
        return res.status(400).send("All fields are required");
      }
      let newQuestionId = await knex("user_questions").insert({
        question_name,
        unstructured_question_body,
        structured_question,
      });
      newQuestion = await knex("user_questions").where({
        id: newQuestionId[0],
      });
    }
    return res.status(201).json(newQuestion[0]);
  } catch (error) {
    console.error("Error creating question: ", error);
    res.status(400).json({ message: `Unable to create new one: ${error}` });
  }
};

const addSubmission = async (req, res) => {
  try {
    const { id: user_question_id } = req.params;
    const { submission_analyses } = req.body;
    const newSubmissionId = await knex("submissions").insert({
      user_question_id,
      submission_analyses,
    });
    const newSubmission = await knex
      .select("*")
      .from("submissions")
      .where({ id: newSubmissionId[0] });
    res.status(201).json(newSubmission[0]);
  } catch (error) {
    console.error(`Error adding submission:${error}`);
    res.status(400).json({ message: `Error adding submission:${error}` });
  }
};
const getOne = async (req, res) => {
  try {
    const { id } = req.params;
    let question = await knex("user_questions").where({ id: id });
    if (!question) {
      return res.status(404).json({ message: "Question with id not found" });
    }
    const submissions = await knex
      .select("*")
      .from("submissions")
      .where({ user_question_id: id });
    return res.status(200).json({ question: question[0], submissions });
  } catch (error) {
    console.error(`Error retrieving question:${error}`);
    res.status(400).json({ message: `Error retrieving question:${error}` });
  }
};
export { index, addOne, getOne, addSubmission };
