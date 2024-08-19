import {
  handleSuccess,
  httpService,
  readAll,
  getById,
  create,
  update,
  setNotActive,
} from "./HttpService";

async function searchMemberByCondition(input) {
  return await httpService.get("/member/search/" + input).then((res) => {
    return handleSuccess(res);
  });
}

async function paginate(params) {
  return await httpService.get("/member/members", { params }).then((res) => {
    return handleSuccess(res);
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
};
