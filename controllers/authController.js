import initKnex from "knex";
import configuration from "../knexfile.js";
import bcrypt from "bcrypt";
const knex = initKnex(configuration);
import jwt from 'jsonwebtoken';

const emailRe =
  /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;
const passwordRegex = /^(?=.*\d).{5,}$/;

const register = async (req, res) => {
    try {
      const { username, email, password } = req.body;
      if (
        !username ||
        !email ||
        !password ||
        !email.match(emailRe) ||
        !password.match(passwordRegex)
      ) {
        return res
          .status(400)
          .json({
            message:
              "Invalid registration: all fields must be filled, valid email must be provided and a password must be at least 5 characters including 1 number",
          });
      }
  
      //check that email and username not already in database.
      const user = await knex('users').select("username", "email").whereILike("email", `${email}`).orWhereILike("username", `${username}`);

      if (user.length) {
        return res.status(409).json({message: "user already exists"})
      }
  
      const hashedPassword = await bcrypt.hash(password, 10);
      const newUserID = await knex("users").insert({ username, email, password: hashedPassword });
      const newUser = await knex("users").select("*").where({id: newUserID[0]})
      res.status(201).json({...newUser[0], password: undefined});
    } catch (error) {
      console.error("couldn't register user: ", error);
      res.status(500).json({ message: "couldn't register user: " + error });
    }
  }

  const login = async (req, res) => {
    try {
        const {username, password} = req.body;
      //check database for user
      const user = await knex('users').select("id", "username", "password").whereILike("username", `${username}`);
      if(!user.length) {
        return res.status(404).json({message: "user not found"})
      }
      //to validate password ->  [returns true/false]
      if (await bcrypt.compare(password, user[0].password)) {
        const token = jwt.sign({
          id: user[0].id
        }, process.env.ACCESS_TOKEN_SECRET)
        return res.status(200).json({token, id: user[0].id})
      }
      return res.status(401).json({message: "incorrect password"})
    } catch (error) {
      console.error("couldn't login user: ", error);
      res.status(500).json({ message: "couldn't login user: " + error });
    }
  }

  export {register, login}