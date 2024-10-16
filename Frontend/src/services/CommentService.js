import {
  httpService,
  handleSuccess,
  readAll,
  getById,
  create,
  update,
  setNotActive,
  processError,
} from "./HttpService";

async function getRecipeComments(recipeId) {
  return await httpService
    .get("/comment/recipeComments/" + recipeId)
    .then((res) => {
      return handleSuccess(res);
    })
    .catch((e) => {
      processError(e);
    });
}

export default {
  readAll,
  create,
  getById,
  update,
  setNotActive,
  getRecipeComments,
};
