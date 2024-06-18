import base from "./base.service";

const instance = base.service(false);

export const login = (loginPayload) => {
  return instance.post("/Account/login-firebase", loginPayload);
};

export const register = (registerPayload) => {
  return instance.post("/Account/register-firebase", registerPayload);
};

export default {
  login,
  register,
};
