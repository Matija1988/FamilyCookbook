import { handleSuccess, httpService, processError } from "./HttpService";

async function RegisterUser(user) {
  return await httpService
    .post("/Register/registerUser", user)
    .then((res) => {
      return handleSuccess(res);
    })
    .catch((e) => {
      return processError(e);
    });
}

export default { RegisterUser };
