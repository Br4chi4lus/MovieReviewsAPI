export class AuthService {
  static verifyTokenIsExpired(): boolean {
    const token = localStorage.getItem('token');

    if (!token) return true;

    try {
      const payloadBase64 = token.split('.')[1];
      const decoded = JSON.parse(atob(payloadBase64));
      const exp = decoded.exp;

      return !exp || exp < Math.floor(Date.now() / 1000);
    } catch {
      return true;
    }
  }
}
