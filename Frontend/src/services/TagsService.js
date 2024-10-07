import {
  httpService,
  handleSuccess,
  readAll,
  getById,
  create,
  update,
  setNotActive,
  processError,
  deleteEntity,
} from "./HttpService";

async function paginate(params) {
  return await httpService
    .get("/tag/tags", { params })
    .then((res) => {
      return handleSuccess(res);
    })
    .catch((e) => {
      return processError(e);
    });
}

export default {
  readAll,
  create,
  getById,
  update,
  setNotActive,
  paginate,
  deleteEntity,
};
