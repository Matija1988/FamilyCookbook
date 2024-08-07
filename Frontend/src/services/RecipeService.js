import {
  httpService,
  handleSuccess,
  readAll,
  getById,
  create,
  update,
  setNotActive,
} from "./HttpService";

async function removeMemberFromRecipe(memberId, recipeId) {
  return await httpService
    .delete("recipe/RemoveMemberFromRecipe/" + memberId + "/" + recipeId)
    .then((res) => {
      return handleSuccess(res);
    });
}

export default {
  readAll,
  getById,
  create,
  update,
  setNotActive,
  removeMemberFromRecipe,
};
