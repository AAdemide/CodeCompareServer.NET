import express from "express";
import { index, addOne, getOne, addSubmission } from "../controllers/userQuestionsController.js";

const router = express.Router();

router.route("/").get(index).post(addOne);
router.route("/:id").get(getOne);
router.route("/:id/submission").post(addSubmission);

export default router;
