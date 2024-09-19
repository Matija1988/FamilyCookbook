import axios, { AxiosError } from "axios";
import { App } from "../constants/constants";

export const httpService = axios.create({
  baseURL: App.URL + "api/v0",
  headers: { "Content-Type": "application/json" },
});

export async function readAll(name) {
  return await httpService
    .get("/" + name)
    .then((res) => {
      return handleSuccess(res);
    })
    .catch((e) => {
      return processError(e);
    });
}

export async function getById(name, id) {
  return await httpService
    .get("/" + name + "/" + id)
    .then((res) => {
      return handleSuccess(res);
    })
    .catch((e) => {
      return processError(e);
    });
}

export async function create(name, entity) {
  return await httpService
    .post("/" + name, entity)
    .then((res) => {
      return handleSuccess(res);
    })
    .catch((e) => {
      return processError(e);
    });
}

export async function update(name, id, entity) {
  return await httpService
    .put("/" + name + "/" + id, entity)
    .then((res) => {
      return handleSuccess(res);
    })
    .catch((e) => {
      return processError(e);
    });
}

export async function setNotActive(name, id) {
  return await httpService
    .put("/" + name + "/", id)
    .then((res) => {
      return handleSuccess(res);
    })
    .catch((e) => {
      return processError(e);
    });
}

export function processError(e) {
  if (!e.response) {
    return {
      ok: false,
      data: [generateMessage("Network issue", "server unresponsive")],
    };
  }
  if (e.code == AxiosError.ERR_NETWORK) {
    return {
      ok: false,
      data: [generateMessage("Network issue", "Try again later")],
    };
  }
  if (e.response.status === 400 && e.response.data.errors) {
    const validationErrors = Object.entries(e.response.data.errors).map(
      ([key, messages]) => ({
        property: key,
        message: messages.join(", "),
      })
    );
    return {
      ok: false,
      data: validationErrors,
    };
  }

  return {
    ok: false,
    data: [generateMessage("Error", "Sum Tin Wong")],
  };
}

function generateMessage(property, message) {
  return { property: property, message: message };
}

export function handleSuccess(res) {
  if (App.DEV) {
    console.table(res.data);
  }
  return { ok: true, data: res.data };
}
