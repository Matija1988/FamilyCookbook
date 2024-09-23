import {
  handleSuccess,
  httpService,
  readAll,
  getById,
  create,
  update,
  setNotActive,
  processError,
} from "./HttpService";

async function searchMemberByCondition(input) {
  return await httpService
    .get("/member/search/" + input)
    .then((res) => {
      return handleSuccess(res);
    })
    .catch((e) => {
      return processError(e);
    });
}

async function paginate(params) {
  return await httpService
    .get("/member/members", { params })
    .then((res) => {
      return handleSuccess(res);
    })
    .catch((e) => {
      return processError(e);
    });
}

async function getByUsername(username) {
  return await httpService
    .get("/member/findByUsername/" + username)
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
  searchMemberByCondition,
  paginate,
  getByUsername,
};
