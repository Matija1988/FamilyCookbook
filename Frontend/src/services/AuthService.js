import { httpService, handleSuccess, processError } from "./HttpService";

async function logInService(userData) {
  return await httpService
    .post("/login/", userData)
    .then((res) => {
      return handleSuccess(res);
    })
    .catch((e) => {
      return processError(e);
    });
}

export default { logInService };
