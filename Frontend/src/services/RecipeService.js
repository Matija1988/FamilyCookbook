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

async function removeMemberFromRecipe(memberId, recipeId) {
  return await httpService
    .delete("recipe/RemoveMemberFromRecipe/" + memberId + "/" + recipeId)
    .then((res) => {
      return handleSuccess(res);
    })
    .catch((e) => {
      processError(e);
    });
}

async function paginate(params) {
  return await httpService
    .get("/recipe/paginate", { params })
    .then((res) => {
      return handleSuccess(res);
    })
    .catch((e) => {
      return processError(e);
    });
}

export default {
  readAll,
  getById,
  create,
  update,
  setNotActive,
  removeMemberFromRecipe,
  paginate,
};
