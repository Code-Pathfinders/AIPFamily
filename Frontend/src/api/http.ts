import axios, { AxiosInstance, AxiosRequestConfig, AxiosResponse } from 'axios'

/**
 * HTTP客户端封装
 */
class HttpClient {
  private instance: AxiosInstance

  constructor() {
    this.instance = axios.create({
      baseURL: '/api',
      timeout: 30000,
      headers: {
        'Content-Type': 'application/json'
      }
    })

    this.setupInterceptors()
  }

  /**
   * 设置拦截器
   */
  private setupInterceptors() {
    // 请求拦截器
    this.instance.interceptors.request.use(
      (config) => {
        return config
      },
      (error) => {
        return Promise.reject(error)
      }
    )

    // 响应拦截器
    this.instance.interceptors.response.use(
      (response: AxiosResponse) => {
        return response.data
      },
      (error) => {
        console.error('API Error:', error)
        return Promise.reject(error)
      }
    )
  }

  /**
   * GET请求
   */
  get<T = any>(url: string, config?: AxiosRequestConfig): Promise<T> {
    return this.instance.get(url, config)
  }

  /**
   * POST请求
   */
  post<T = any>(url: string, data?: any, config?: AxiosRequestConfig): Promise<T> {
    return this.instance.post(url, data, config)
  }

  /**
   * PUT请求
   */
  put<T = any>(url: string, data?: any, config?: AxiosRequestConfig): Promise<T> {
    return this.instance.put(url, data, config)
  }

  /**
   * DELETE请求
   */
  delete<T = any>(url: string, config?: AxiosRequestConfig): Promise<T> {
    return this.instance.delete(url, config)
  }
}

export const http = new HttpClient()
