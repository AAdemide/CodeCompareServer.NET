import express from "express";
import initKnex from "knex";
import configuration from "../knexfile.js";
const knex = initKnex(configuration);
import axios from "axios";
import "dotenv/config";

const router = express.Router();
const { LAPI_URL } = process.env;
router.get("/", (_req, res) => {
  res.status(200).send("Valid routes are: /daily, /random, /:slug");
});
router.get("/daily", async (_req, res) => {
  try {
    const {
      data: {
        data: { activeDailyCodingChallengeQuestion: dailyQuestion },
      },
    } = await axios.get(`${LAPI_URL}/dailyQuestion`);
    return res.status(200).json(dailyQuestion);
  } catch (error) {
    console.error("There was an error retrieving the daily question: ", error);
    return res
      .status(500)
      .json({
        message: `There was an error retrieving the daily question:  ${error}`,
      });
  }
});
router.get("/random", async (_req, res) => {
  try {
    const {
      data: { problemsetQuestionList: problems },
    } = await axios.get(`${LAPI_URL}/problems?limit=500`);
    // console.log(problems);
    const randomQuestion =
      problems[Math.floor(Math.random() * problems.length)];
    //get question
    const { titleSlug: slug } = randomQuestion;
    const { data: question } = await axios.get(
      `${LAPI_URL}/select?titleSlug=${slug}`
    );
    if (!question.questionId) {
      return res
        .status(400)
        .json({ message: `Could not find question with titleSlug:  ${slug}` });
    }
    const submissions = await knex
      .select(["submissions.*", "user_questions.leetcode_id", "user_questions.id"])
      .from("submissions")
      .where({leetcode_id: question.questionId})
      .join("user_questions", "submissions.user_question_id", "user_questions.id");
    return res.status(200).json({ question, submissions });
  } catch (error) {
    console.error("There was an error retrieving a random question: ", error);
    return res
      .status(500)
      .json({
        message: `There was an error retrieving a random question:  ${error}`,
      });
  }
});
router.get("/:slug", async (req, res) => {
  try {
    const { slug } = req.params;
    const { data: question } = await axios.get(
      `${LAPI_URL}/select?titleSlug=${slug}`
    );
    if (!question.questionId) {
      return res
        .status(400)
        .json({ message: `Could not find question with titleSlug:  ${slug}` });
    }
    const submissions = await knex
      .select(["submissions.*", "user_questions.leetcode_id"])
      .from("submissions")
      .where({leetcode_id: question.questionId})
      .join("user_questions", "submissions.user_question_id", "user_questions.id");
      const userQuestionId = await knex.select("id").from("user_questions").where({leetcode_id: question.questionId});
    return res.status(200).json({ userQuestionId: userQuestionId[0]?.id, question, submissions });
  } catch (error) {
    console.error("There was an error retrieving the question: ", error);
    return res
      .status(500)
      .json({
        message: `There was an error retrieving the question:  ${error}`,
      });
  }
});

export default router;
