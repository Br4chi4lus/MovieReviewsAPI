import { AuthService } from '@/services/auth.service.ts';
import { ref } from 'vue';

export const isAuthenticated = ref(!AuthService.verifyTokenIsExpired());
export const userIsModOrAdmin = ref<boolean>(AuthService.isModOrAdmin());
