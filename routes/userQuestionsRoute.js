import express from "express";
import { index, addOne, getOne, addSubmission } from "../controllers/userQuestionsController.js";

const router = express.Router();

const userMiddleware = (req, res, next) => {
    const authHeader = req.headers['authorization'];
    const token = authHeader && authHeader.split(" ")[1];
    if(!token) return res.status(401).json({message: "unauthorized"});
  
    JsonWebTokenError.verify(token, process.env.ACCESS_TOKEN_SECRET, (err, user) => {
      if (err) return res.status(403).json({message: "token expired"});
      console.log(user)
      req.user = user
    });
    next()
  }

router.route("/").get(index).post(addOne);
router.route("/:id").get(getOne);
router.route("/:id/submission").post(addSubmission);

export { router as default, userMiddleware};
