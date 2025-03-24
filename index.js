import 'dotenv/config';
import express from "express";
import userQuestionsRoute from "./routes/userQuestionsRoute.js";
import lapiRoute from "./routes/lapiRoute.js";
import cors from 'cors';

const app = express();
const {CORS_ORIGIN} = process.env
app.use(cors({origin: CORS_ORIGIN}));
const {PORT} = process.env || 5050;

app.use(express.json());

app.get("/", (_req, res) => {
  res.send("Welcome to my API. Valid routes are /userQuestions and /collections");
});

app.use("/userQuestions", userQuestionsRoute);
app.use("/lapi", lapiRoute);
// app.use("/inventories", inventoryRoutes);

app.listen(PORT, () => {
  console.log(`running at http://localhost:${PORT}`);
});
