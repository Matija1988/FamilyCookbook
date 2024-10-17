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
  paginate,
} from "./HttpService";

async function getByText(input) {
  return await httpService
    .get("/tag/getByText/" + input)
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
  getByText,
};
