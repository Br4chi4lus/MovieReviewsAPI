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

  static decodeToken(): Record<string, never> | null {
    const token = localStorage.getItem('token');

    if (!token) return null;

    try {
      const payloadBase64 = token.split('.')[1];
      const decoded = JSON.parse(atob(payloadBase64));
      return decoded;
    } catch {
      return null;
    }
  }

  static getUserRole(): string | null {
    const decodedToken = AuthService.decodeToken();
    if (!decodedToken) return null;

    return decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || null;
  }

  static isModOrAdmin(): boolean {
    if (AuthService.verifyTokenIsExpired()) return false;

    const role = AuthService.getUserRole();
    if (!role) return false;

    if (role.toLowerCase() === 'admin' || role.toLowerCase() === 'moderator') return true;

    return false;
  }
}
