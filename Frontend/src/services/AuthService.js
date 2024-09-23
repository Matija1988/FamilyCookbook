import { httpService, handleSuccess, processError } from "./HttpService";

export async function logInService(userData) {
  return await httpService
    .post("login", userData)
    .then((res) => {
      return handleSuccess(res);
    })
    .catch((e) => {
      return {
        error: true,
        data: [{ property: "Authorization", message: e.response.data }],
      };
    });
}
