import { handleSuccess, httpService, processError } from "./HttpService";

async function SearchRecipeByTag(tag) {
  return await httpService
    .get("Search/search?text=" + tag)
    .then((res) => {
      return handleSuccess(res);
    })
    .catch((e) => {
      processError(e);
    });
}

export default { SearchRecipeByTag };
